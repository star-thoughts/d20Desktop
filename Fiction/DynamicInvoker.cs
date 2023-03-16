using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Fiction
{
    /// <summary>
    /// Object used to construct and interoperate with a given type
    /// </summary>
    public class DynamicInvoker
    {
        #region Constructors
        public DynamicInvoker(Type type)
        {
            _type = type;
        }
        #endregion
        #region Member Variables
        private Type _type;
        private ConstructorDelegate0 _constructor0;

        public delegate object ConstructorDelegate0();
        #endregion
        #region Methods
        public ConstructorDelegate0 GetConstructor()
        {
            if (_constructor0 == null)
            {
                DynamicMethod method = new DynamicMethod("Construct " + _type.Name, _type, Array.Empty<Type>());
                ConstructorInfo constructor = _type.GetConstructor(Array.Empty<Type>());

                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Newobj, constructor);
                il.Emit(OpCodes.Ret);

                _constructor0 = (ConstructorDelegate0)method.CreateDelegate(typeof(ConstructorDelegate0));
            }

            return _constructor0;
        }
        #endregion
    }
}
