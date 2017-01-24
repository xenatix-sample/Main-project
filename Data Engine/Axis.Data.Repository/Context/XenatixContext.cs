//using Axis.Data.Repository.Migrations;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Axis.Data.Repository.Context
{
    public class XenatixContext : DbContext
    {
        public XenatixContext()
            : base("name=XenatixDBConnection")
        {
        }
    }
}