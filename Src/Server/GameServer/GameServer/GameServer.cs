﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Configuration;

using System.Threading;

using Network;
using GameServer.Services;

namespace GameServer
{
    class GameServer
    {
        Thread thread;
        bool running = false;
        NetService netWork;

        public bool Init()
        {
            netWork = new NetService();
            netWork.Init(8000);

            MyKeyService.Instance.Init();
            DBService.Instance.Init();
            //var a = DBService.Instance.Entities.Characters.Where(s => s.TID == 2);
            //if (a != null) Console.WriteLine("{0}", a.FirstOrDefault<TCharacter>().Name);
            //else Console.WriteLine("a is null");
            thread = new Thread(new ThreadStart(this.Update));
            return true;
        }

        public void Start()
        {
            netWork.Start();
            MyKeyService.Instance.Start();
            running = true;
            thread.Start();
        }


        public void Stop()
        {
            netWork.Stop();
            MyKeyService.Instance.Stop();
            running = false;
            thread.Join();
        }

        public void Update()
        {
            while (running)
            {
                Time.Tick();
                Thread.Sleep(100);
                //Console.WriteLine("{0} {1} {2} {3} {4}", Time.deltaTime, Time.frameCount, Time.ticks, Time.time, Time.realtimeSinceStartup);
            }
        }
    }
}
