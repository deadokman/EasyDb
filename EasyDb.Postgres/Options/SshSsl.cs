using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EDb.Interfaces;

namespace EasyDb.Postgres.Options
{
    public class SshSsl : EdbSourceOption
    {
        public SshSsl()
        {
            OptionsDefinitionName = "Ssl/Ssh";
        }
    }
}
