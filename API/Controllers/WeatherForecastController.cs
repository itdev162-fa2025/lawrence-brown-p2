using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly DataContext _context;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet (Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public ActionResult<WeatherForecast> Create([FromBody] WeatherForecast model)
    {
        //var tempc=Convert.ToInt16(temp);s
        //View in your VS Code console - fyi
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine ("Insert a new WeatherForecast");
    
        double tempc = model.TemperatureC;
        string summ = "";
        double tempf = 32 + tempc / 0.5556;
        if (tempf <= 32){ summ = "Freezing"; }
        if (tempf > 32 && tempf <= 40) { summ = "Bracing"; ;}
        if (tempf > 40 && tempf <= 50) { summ = "Chilly"; ; }
        if (tempf > 50 && tempf <= 60) { summ = "Cool"; ; }
        if (tempf > 60 && tempf <= 70) { summ = "Mild"; ; }
        if (tempf > 70 && tempf <= 80) { summ = "Warm"; ; }
        if (tempf > 80 && tempf <= 90) { summ = "Balmy"; ; }
        if (tempf > 90 && tempf <= 100) { summ = "Hot"; ; }
        if (tempf > 100 && tempf <= 110) { summ = "Sweltering"; ; }
        if (tempf > 110 && tempf <= 140) { summ = "Scorching"; ; }

        var forecast = new WeatherForecast()
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = model.TemperatureC,
            Summary = summ
        };

        model.Summary = summ;
        Console.WriteLine(model.Summary);


        _context.WeatherForecasts.Add(forecast);
        var success = _context.SaveChanges() > 0;

        if (success)
        {
            return forecast;
        }

        throw new Exception("Error creating WeatherForecast");
    }
}