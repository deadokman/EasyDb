using System;
using System.Data;
using System.Drawing;
using System.Windows.Media;
using EDb.Interfaces.Objects;

namespace EDb.Interfaces
{
    using System.Linq;

    using EDb.Interfaces.Options;

    public abstract class EdbDatasourceModule : MarshalByRefObject
    {
        /// <summary>
        /// Имя модуля СУБД
        /// </summary>
        public abstract string DatabaseName { get; }

        /// <summary>
        /// Типы поддерживаемых объектов базы данных
        /// </summary>
        public abstract SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Значек базы данных
        /// </summary>
        public abstract byte[] DatabaseIcon { get; }

        /// <summary>
        /// Option objects
        /// Вернуть объекты настроек
        /// </summary>
        /// <returns></returns>
        public abstract EdbSourceOption[] GetDefaultOptionsObjects();

        /// <summary>
        /// Get option defenition objects
        /// </summary>
        /// <returns></returns>
        public virtual ModuleOptionDefinition[] GetOptionsDefenitions()
        {
            return this.GetDefaultOptionsObjects().Select(so => so.ToOptionDefinition()).ToArray();
        }

        public virtual Guid ModuleGuid { get; private set; }

        public virtual Version Version { get; private set; }

        public virtual void SetVersion(Version version)
        {
            Version = version;
        }

        public virtual void SetGuid(Guid guid)
        {
            ModuleGuid = guid;
        }
    }
}
