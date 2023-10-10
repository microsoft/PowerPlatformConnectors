# Telegram Bot
This is to control Telegram Bot

## Publisher: Woong Choi

## Supported Operations

### Get Updates
Use this method to receive incoming updates using long polling

### Get Me
Returns basic information about the bot in form of a User object. A simple method for testing your bot's auth token.

### Send Message
Use this method to send text messages

### Send Photo
Use this method to send photos

### Get Chat
Use this method to get up to date information about the chat

## Getting Started
Visit [Telegram Bot API](https://core.telegram.org/bots/api) page for further details.

## Limitation
It uses token into path. You need to secure it as it will be plain text by default. In power automate, you can turn on secure input and use Azure Key Vault to secure bot token detail.