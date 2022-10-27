using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Entities
{
    public interface IEntityController
    {
        void PlayAnim(string name);
        void SetStandBy(bool standby);
    }
}