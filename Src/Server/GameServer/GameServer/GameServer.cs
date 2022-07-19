using System;
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
using GameServer.Managers;

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

            DataManager.Instance.Load();
            MapManager.Instance.Init();
            MyKeyService.Instance.Init();
            DBService.Instance.Init();
            UserService.Instance.Init();
            MapService.Instance.Init();
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
