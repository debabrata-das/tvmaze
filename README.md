# tvmaze
TV Maze Data Scrapper - scrapes all tv shows, including casts list, from http://www.tvmaze.com/api endpoint, persists in local SQL Server database, and provides WebAPI for reading back the scraped data.

The API will satisfy the following requirements:
1. Provide a list of all tv shows containing the id of the TV show and cast details
2. The list of the cast must be ordered by birthday descending

Example response:
```json
[
  {
    "id": 1,
    "name": "Game of Thrones",
    "cast": [
        {
          "id": 9,
          "name": "Dean Norris",
          "birthday": "1963-04-08"
        },
        {
          "id": 7,
          "name": "Mike Vogel",
          "birthday": "1979-07-17"
        }
        ]
        },
  {
    "id": 4,
    "name": "Big Bang Theory",
    "cast": [
        {
          "id": 6,
          "name": "Michael Emerson",
          "birthday": "1950-01-01"
        }
    ]
  } 
]
```

## Technologies

1. .Net Core 2.1 and Entity Framework Core
2. Sql Server

## How to execute

 1. Run TvMazeDataIntegrator.Console and scrape the data from http://www.tvmaze.com/api
 2. Run TvInfoApiService
 3. Navigate to http://localhost:11972/api/shows in a browser. For filtering results append: `pageNumber=1&pageSize=10` parameters at the end of the endpoint.
 4. Alternatively navigate to http://localhost:11972/swagger/index.html for testing API on any browser
  