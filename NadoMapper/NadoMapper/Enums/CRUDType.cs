using System;
using System.Collections.Generic;
using System.Text;

namespace NadoMapper.Enums
{
    /// <summary>
    /// Dictates the type of CRUD operation being performed, and therefore which parameters to keep or omit depending on selected property conventions
    /// </summary>
    public enum CRUDType
    {
        None,
        Create,
        Read,
        Update,
        Delete
    }
}
