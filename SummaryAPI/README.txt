Summary API Documentation
Author: Joseph Bjorkman

Summary:
	This API was created as the result of a request from PrologMobile as part of their job application. 
	The API will return a JSON summary of all of the Organizations from a backend server

	The returned summary has the form of:
	[{
		"id": "{Organization1 ID}",
		"name": "{Organization Name}",
		"blacklistTotal": "{Total Blacklisted Phones}",
		"totalCount": "{Total Phone Conut}",
		"users": [{

					"id": "{User1 ID}",
					"email": "{User1 Email}",
					"phoneCount": {User1 Phone Count}
				},{
					"id": "{User2 ID}",
					"email": "{User2 Email}",
					"phoneCount": {User2 Phone Count}
				}]

	},{
		"id": "{Organization2 ID}",
		…
	}]

	Right now, this API is not hoseted on a domain and so has to be run and accessed on localhost. The URL will be http://localhost:53752/summary, 
	unless the url is changed in launchSettings.json. The easiest way to run and access it is to open up the Visual Studio Solution and run it in debug mode.
	It will open a browser tab and will appear blank for a little while, and then show the summary.