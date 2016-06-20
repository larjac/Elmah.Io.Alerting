# Elmah.Io.Alerting

An extension to Elmah.io (https://github.com/elmahio/elmah.io) that allows you to set up alert policies for anomalities in your Elmah.io logs. 

Provided alerting channels are currently Slack (requires incoming webhook) and Gmail smtp.

Example usage:
```
AlertPolicy
    .Create("Recent error rate in past 15 min")
    .ForLogs(_logIdApp1, _logIdApp2)
    .When(p => (DateTime.UtcNow - p.Min(dt => dt.DateTime)).TotalMinutes < 15)
    .ToChannels(new SlackChannel(_slackHookUrl), new GmailSmtpChannel(_smtpUsername, _smtpPassword, "email@example.com"))
    .WithIntervalSeconds(30);
```
