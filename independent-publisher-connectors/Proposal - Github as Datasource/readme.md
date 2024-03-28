## Github as a datassource

This connector allows you to grab a raw MD file from your github repo, and pull it in as data to then be manipulated based on your markdown format.

## Publisher: Nathalie Leenders | Wortell

## Pre-requisites

You will need the following to proceed:

- A Microsoft PowerApps or Microsoft Flow plan with custom connector feature
- A premium license

## How to use the connector

There are 5 parameters built in

1 Github name
2 Repo name
3 Folder name
4 Folder2 name
5 Markdownfile name

- Github name is your useraccount for github, or from the account youre trying to pull data from.

-  Take note, it is open source so it doesnt need any authentication on github.

- Repo name is the name of your repo.

- Foldername is the name of the first folder

- Foldername2 is the underlying folders. If there are more than use use foldername/foldername2/foldername3 in this parameter if needed.

- Then the name of the markdown file you're trying to grab.

- Once done, hit submit, and it will grab the raw markdown data for you.

You can now use data operations or redex to further manipulate your data.
