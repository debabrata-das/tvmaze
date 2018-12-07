using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using TvInfo.Persistence.Models.DTO;
using TvMazeDataReader.Interfaces;

namespace TvMazeDataReader
{
	public class TvMazeService : ITvMazeService
	{
		private readonly IJsonApiDataReader _jsonApiDataReader;
		private readonly ILogger<TvMazeService> _logger;

		public TvMazeService(IJsonApiDataReader jsonJsonApiDataReader, ILogger<TvMazeService> logger)
		{
			_jsonApiDataReader = jsonJsonApiDataReader;
			_logger = logger;
		}

	    public async Task<ShowDto> GetShowDataById(int showId,
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var jshow = await _jsonApiDataReader.GetJsonData<dynamic>(
					new Uri($"/shows/{showId}?embed=cast", UriKind.Relative), cancellationToken);

			var show = new ShowDto
			{
				Id = jshow.id,
				Name = jshow.name
			};
			var embedded = jshow._embedded;
			if (embedded is null)
			{
				_logger.LogError($"No embedded data returned for show {showId}.");
			}
			else
			{
				var jcast = embedded.cast;
				if (jcast is null)
				{
					_logger.LogError($"No cast data found embedded for show {showId}.");
				}
				else
				{
					foreach (var container in jcast)
					{
						var person = container.person;
						var personDto = new PersonDto
						{
							Id = person.id,
							Name = person.name,
							Birthdate = person.birthday,
						};

						show.Cast.Add(personDto);
					}
				}
			}

			return show;
		}

		public async Task<List<ShowDto>> GetShowsByPage(int pageNumber, 
			CancellationToken cancellationToken = default(CancellationToken))
		{
			var result = new List<ShowDto>();
			var shows = await _jsonApiDataReader.GetJsonData<JArray>(
					new Uri($"/shows?page={pageNumber}", UriKind.Relative), cancellationToken);

			foreach (dynamic jshow in shows)
			{
				var show = new ShowDto
				{
					Id = jshow.id,
					Name = jshow.name
				};
				result.Add(show);
			}

			_logger.LogInformation($"Getting {result.Count} Shows for pageNumber {pageNumber}");
			return result;
		}
	}
}