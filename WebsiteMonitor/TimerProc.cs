using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteMonitor.Test
{
    public class TimerContainer : IDisposable
    {

        public int TickCount = 0;
        public int FailureCount = 0;
        public Timer Timer { get; set; }
        public TimerContainer()
        {
            Timer = new System.Threading.Timer(TimerProc, null, 500, 30000);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
            Console.WriteLine("Executed " + TickCount);
            Console.WriteLine("Failed " + FailureCount);
        }

        public async void TimerProc(object state)
        {
            OpenWebsite();
        }

        public async void OpenWebsite()
        {
            ++TickCount;
            Console.WriteLine("tick {0}", TickCount);

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Webkit.LaunchAsync();
            var page = await browser.NewPageAsync();

            try
            {
                await page.GotoAsync("https://www.google.com");
                await page.ScreenshotAsync(new() { Path = "googlelogo_color_272x92dp.png" });
                Console.WriteLine("Success");
            }
            catch (Microsoft.Playwright.PlaywrightException)
            {
                FailureCount++;
                Console.WriteLine("ERROR!!! Tick " + TickCount + " has timed out.");

            }

            await browser.CloseAsync();

        }
    }
}
