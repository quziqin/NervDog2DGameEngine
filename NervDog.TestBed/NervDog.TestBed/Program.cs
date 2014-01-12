using System;

namespace NervDog.TestBed
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (NervDogGame game = new NervDogGame())
            {
                game.Run();
            }
        }
    }
#endif
}

