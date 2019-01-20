using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using EDb.Interfaces.Annotations;
using EDb.Interfaces.Validation;

namespace EDb.Interfaces
{
    /// <summary>
    /// Interface for datasource options
    /// Класс настроек для источника данных
    /// </summary>
    public abstract class EdbSourceOption : ValidationViewModelBase
    {
 
        [Browsable(false)]
        public string OptionsDefinitionName { get; protected set; }    
    }
}
