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
        //var tempc=Convert.ToInt16(temp);
        //View in your VS Code console - fyi
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine ("Insert a new WeatherForecast");
    
        int Id = model.Id;
        double tempc = model.TemperatureC;
        string summ = "";
        double tempf = 32 + tempc / 0.5556;
        bool success = false;

        if (tempf <= 32){ summ = "Freezing"; }
        if (tempf > 32 && tempf <= 40) { summ = "Bracing";}
        if (tempf > 40 && tempf <= 50) { summ = "Chilly"; }
        if (tempf > 50 && tempf <= 60) { summ = "Cool"; }
        if (tempf > 60 && tempf <= 70) { summ = "Mild"; }
        if (tempf > 70 && tempf <= 80) { summ = "Warm"; }
        if (tempf > 80 && tempf <= 90) { summ = "Balmy"; }
        if (tempf > 90 && tempf <= 100) { summ = "Hot"; }
        if (tempf > 100 && tempf <= 110) { summ = "Sweltering"; }
        if (tempf > 110 && tempf <= 140) { summ = "Scorching"; }

        var forecast = new WeatherForecast()
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            TemperatureC = model.TemperatureC,
            Summary = summ
        };

        model.Summary = summ;
        Console.WriteLine(model.Summary);


        _context.WeatherForecasts.Add(forecast);

        /* //This queries by Id, if found Removes Record from entity database, then saves changes
        var wfc3 = new WeatherForecast();

        if ((WeatherForecast) _context.WeatherForecasts.Where(i => i.Id == Id) != null) {
            wfc3 = (WeatherForecast)_context.WeatherForecasts.Where(i => i.Id == Id);
            _context.WeatherForecasts.Remove(wfc3);
            success = _context.SaveChanges() > 0; 
        } else {
            Console.WriteLine("Not Found");
        }

        //This queries by Id, if found Updates found record in entity database, then saves changes
        if ((WeatherForecast)_context.WeatherForecasts.Where(i => i.Id == Id) != null)
        {
            wfc3 = (WeatherForecast)_context.WeatherForecasts.Where(i => i.Id == Id);
            wfc3.Date = model.Date;
            wfc3.TemperatureC = model.TemperatureC;
            wfc3.Summary = summ;
            _context.WeatherForecasts.Update(wfc3);
            success = _context.SaveChanges() > 0; 
        }
        else
        {
            Console.WriteLine("Not Found");
        } 

        
        // Other possible method syntaxes I experimented from follow below:
        var wfc1 = (WeatherForecast) _context.WeatherForecasts.Where(i => i.Id == Id);
        var wfc2 = _context.WeatherForecasts.Find(forecast);
        _context.WeatherForecasts.Remove(wfc1);
        _context.WeatherForecasts.Update(wfc1); */

        success = _context.SaveChanges() > 0; 

        if (success)
        {
            return forecast;
        }

        throw new Exception("Error creating WeatherForecast");
    }

    [HttpDelete]
    public ActionResult<WeatherForecast> Delete([FromBody] WeatherForecast model)
    {
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine("Delete a WeatherForecast");

        bool success = false;
        bool found = true;




        //This queries by Id, if found Removes Record from entity database, then saves changes
        var wf = new WeatherForecast();
        foreach (var item in _context.WeatherForecasts)
        {
            if (item.Id == model.Id)
            {
                int Id = model.Id;
                double tempc = model.TemperatureC;
                string summ = "";
                double tempf = 32 + tempc / 0.5556;
 
                if (tempf <= 32) { summ = "Freezing"; }
                if (tempf > 32 && tempf <= 40) { summ = "Bracing"; }
                if (tempf > 40 && tempf <= 50) { summ = "Chilly"; }
                if (tempf > 50 && tempf <= 60) { summ = "Cool"; }
                if (tempf > 60 && tempf <= 70) { summ = "Mild"; }
                if (tempf > 70 && tempf <= 80) { summ = "Warm"; }
                if (tempf > 80 && tempf <= 90) { summ = "Balmy"; }
                if (tempf > 90 && tempf <= 100) { summ = "Hot"; }
                if (tempf > 100 && tempf <= 110) { summ = "Sweltering"; }
                if (tempf > 110 && tempf <= 140) { summ = "Scorching"; }


                model.Summary = summ;
                Console.WriteLine(model.Summary);


                wf = item;
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Date);
                Console.WriteLine(item.TemperatureC);
                Console.WriteLine(item.TemperatureF);
                Console.WriteLine(item.Summary);
                Console.WriteLine();
                Console.WriteLine("Deleted DataBase Record Above");
                _context.WeatherForecasts.Remove(item);
                success = _context.SaveChanges() > 0;
            }
        }


        if (success)
        {
            return wf;
        }
        else
        {
            Console.WriteLine("Not Found");
            //return wf;
        }

        return wf;
        //throw new Exception("Error Deleting WeatherForecast in DataBase");

    }

    [HttpPut]
    public ActionResult<WeatherForecast> Update([FromBody] WeatherForecast model)
    {
        Console.WriteLine($"Database path: {_context.DbPath}");
        Console.WriteLine("Update a WeatherForecast");

        int Id = model.Id;
        double tempc = model.TemperatureC;
        string summ = "";
        double tempf = 32 + tempc / 0.5556;
        bool success = false;
        bool found =  true;

        if (tempf <= 32) { summ = "Freezing"; }
        if (tempf > 32 && tempf <= 40) { summ = "Bracing"; }
        if (tempf > 40 && tempf <= 50) { summ = "Chilly"; }
        if (tempf > 50 && tempf <= 60) { summ = "Cool"; }
        if (tempf > 60 && tempf <= 70) { summ = "Mild"; }
        if (tempf > 70 && tempf <= 80) { summ = "Warm"; }
        if (tempf > 80 && tempf <= 90) { summ = "Balmy"; }
        if (tempf > 90 && tempf <= 100) { summ = "Hot"; }
        if (tempf > 100 && tempf <= 110) { summ = "Sweltering"; }
        if (tempf > 110 && tempf <= 140) { summ = "Scorching"; }


        model.Summary = summ;
        //Console.WriteLine(model.Summary);

        //This queries by Id, if found Removes Record from entity database, then saves changes
        var wf = new WeatherForecast();
        foreach (var item in _context.WeatherForecasts)
        {
            if(item.Id == model.Id)
            {
                item.TemperatureC = model.TemperatureC;
                item.Summary = model.Summary;
                wf=item;
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Date);
                Console.WriteLine(item.TemperatureC);
                Console.WriteLine(item.TemperatureF);
                Console.WriteLine(item.Summary);
                Console.WriteLine();
                Console.WriteLine("Updated DataBase Record Above");
                _context.WeatherForecasts.Update(item);
                success = _context.SaveChanges() > 0;
            }
        }


        if (success)
        {
            return wf;
        } else {
            Console.WriteLine("Not Found");
        }
        return wf;
        //throw new Exception("Error Finding / Updating WeatherForecast in DataBase");


    }
}