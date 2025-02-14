# GOV.UK Notify Connecter
GOV.UK Notify (Notify) is a UK government service for sending letters, emails and sms messages.
This connector builds connections to its various end points.

## Pre-requisites
You will need the following to proceed:
- A Power Automate plan, with access to Premium connectors
- A Notify account

## Getting Credentials
Once you have logged in to your Notify account, navigate to "API integration" on the left hand navbar.
Select "API Keys" from the tabs presented.
Click "Create API Key" and create an appropriate API key.

When creating a connection with this connector, copy the full API key (of the format {service name}-{service id}-{api secret}) into the connection API key field.

## Supported operations
### Sending messages
- Send a text message
- Send an email
- Send a file by email
- Send a letter
- Send a pre-compiled letter

### Other operations
- Get the status of one (or more) messages
- Get the PDF of a letter notification
- Get the details of one (or more) templates
- Get the details of received messages

![image](https://github.com/user-attachments/assets/8f6756bd-db4b-42aa-9f86-9bec4de0d503)

## Documentation
The Notify API is documented [here](https://docs.notifications.service.gov.uk/rest-api.html).
Personalisation should be passed as an object with the necessary fields for the specified template.
