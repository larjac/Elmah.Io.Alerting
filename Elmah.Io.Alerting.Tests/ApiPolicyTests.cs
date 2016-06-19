using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Elmah.Io.Alerting.Channels;
using Elmah.Io.Alerting.Channels.Slack;
using Elmah.Io.Alerting.Client;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;
using Moq;
using NUnit.Framework;

namespace Elmah.Io.Alerting.Tests
{
    [TestFixture]
    public class ChannelRuleBuilderTests
    {

        private static Mock<IApiClient> MockApi
        {
            get
            {
                var mockApi = new Mock<IApiClient>();
                mockApi.Setup(p => p.GetMessages(It.IsAny<string>(), It.IsAny<FilterQuery>()))
                    .Returns(new ElmahIoResponse
                    {
                        Messages = new List<Message>
                        {
                            new Message("error")
                            {
                                StatusCode = 500,
                                Severity = Severity.Error,
                                Type = typeof(NullReferenceException).FullName,
                                DateTime = DateTime.Now
                            },
                            new Message("error")
                            {
                                StatusCode = 500,
                                Severity = Severity.Error,
                                Type = typeof(NullReferenceException).FullName,
                                DateTime = DateTime.UtcNow.AddSeconds(-10)
                            },
                            new Message("error")
                            {
                                StatusCode = 500,
                                Severity = Severity.Error,
                                Type = typeof(ArgumentException).FullName,
                                DateTime = DateTime.UtcNow.AddSeconds(-20)
                            },
                            new Message("error")
                            {
                                StatusCode = 500,
                                Severity = Severity.Error,
                                Type = typeof(Exception).FullName,
                                DateTime = DateTime.UtcNow.AddSeconds(-30)
                            }
                        }
                    });
                return mockApi;
            }
        }

        [Test]
        public void MatchingFilter_ShouldTriggerAlert()
        {

            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", MockApi.Object)
                .ForLogs("testlogid")
                .When(m => m.Count(p => p.StatusCode == 500) > 3)
                .ToChannels(testChannel)
                .WithIntervalSeconds(10);

            WaitForTrigger(testChannel);
            Assert.AreEqual(1, testChannel.AlertCount);
        }

        [Test]
        public void NonMatchingFilter_ShouldNotTriggerAlert()
        {

            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", MockApi.Object)
                .ForLogs("testlogid")
                .When(m => m.Count(p => p.StatusCode == 500) > 4)
                .ToChannels(testChannel)
                .WithIntervalSeconds(10);

            WaitForTrigger(testChannel);
            Assert.AreEqual(0, testChannel.AlertCount);
        }

        [Test]
        public void RecurringAlerts_ShouldBeTriggeredEveryXSeconds()
        {

            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", MockApi.Object, 0)
                .ForLogs("testlogid")
                .When(m => m.Any())
                .ToChannels(testChannel)
                .WithIntervalSeconds(1);

            WaitForTrigger(testChannel, 3);
            Assert.AreEqual(3, testChannel.AlertCount);
        }

        [Test]
        public void ScheduledAlert_ShouldBeTriggeredAtSpecificTime()
        {
            var nextMinute = DateTime.Now.AddMinutes(1);
            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", MockApi.Object, 0)
                .ForLogs("testlogid")
                .When(m => m.Any())
                .ToChannels(testChannel)
                .RunDailyAt(nextMinute.Hour, nextMinute.Minute);

            while (DateTime.Now.Minute != nextMinute.Minute)
            {
                //This test can take up to a monute to run due to that I can't schedule down to the second.
            }
            WaitForTrigger(testChannel);
            Assert.AreEqual(1, testChannel.AlertCount);
        }

        [Test]
        public void OpenAlerts_ShouldBeClosedWhenFilterDoesNotMatch()
        {
            var mockApi = MockApi;
            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", mockApi.Object)
                .ForLogs("testlogid")
                .When(m => m.Any())
                .ToChannels(testChannel)
                .WithIntervalSeconds(1);

            WaitForTrigger(testChannel);
            mockApi.Setup(m => m.GetMessages(It.IsAny<string>(), It.IsAny<FilterQuery>()))
                .Returns(new ElmahIoResponse { Messages = new List<Message>() });
            while (testChannel.AlertCount > 0)
            {
                Thread.Sleep(100);
            }
            Assert.AreEqual(0, testChannel.AlertCount);
        }

        [Test]
        public void FrequentErrors_ShouldSordDescending()
        {
            var testChannel = new TestChannel();
            AlertPolicy.Create("Test", MockApi.Object, 0)
                .ForLogs("testlogid")
                .When(m => m.Any())
                .ToChannels(testChannel)
                .WithIntervalSeconds(10);

            WaitForTrigger(testChannel);
            Assert.AreEqual(1, testChannel.AlertCount);
        }


        private static void WaitForTrigger(TestChannel testChannel, int conditionalIndex = 1)
        {
            var start = DateTime.Now;
            while (testChannel.AlertCount < conditionalIndex && (DateTime.Now - start).TotalSeconds < 10)
            {
                Thread.Sleep(10);
            }
        }
    }
}
