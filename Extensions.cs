using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Controls;

namespace BotWithUI
{
    // these delegates are used for invoking respective methods between threads
    public delegate void SetPropertyDelegate<TCtl, TProp>(TCtl control, Expression<Func<TCtl, TProp>> propexpr, TProp value) where TCtl : Control;
    public delegate TProp GetPropertyDelegate<TCtl, TProp>(TCtl control, Expression<Func<TProp>> propexpr) where TCtl : Control;
    public delegate void InvokeActionDelegate<TCtl>(TCtl control, Delegate dlg, params object[] args) where TCtl : Control;
    public delegate TResult InvokeFuncDelegate<TCtl, TResult>(TCtl control, Delegate dlg, params object[] args) where TCtl : Control;

    // these are various convenience extensions for thread-safe ui mutation
    public static class Extensions
    {
        // this method modifies specified property, assigning it the given value
        // usage is control.SetProperty(x => x.Property, value)
        public static void SetProperty<TCtl, TProp>(this TCtl control, Expression<Func<TCtl, TProp>> propexpr, TProp value) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (propexpr == null)
                throw new ArgumentNullException(nameof(propexpr));

            // get the control's dispatcher
            var disp = control.Dispatcher;

            // check if dispatching is required
            if (!disp.CheckAccess())
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                disp.Invoke(new SetPropertyDelegate<TCtl, TProp>(SetProperty), control, propexpr, value);
                return;
            }

            // get the body of the expression, check if it's an expression that 
            // results in a class member being passed
            if (!(propexpr.Body is MemberExpression propexprm))
                throw new ArgumentException("Invalid member expression.", nameof(propexpr));

            // get the member from the expression body, and check if it's a property
            var prop = propexprm.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException("Invalid property supplied.", nameof(propexpr));

            // finally, set the value of the property to the supplied one
            prop.SetValue(control, value);
        }

        // this method reads the value of specified property, and returns it
        // usage is control.GetProperty(x => x.Property)
        public static TProp GetProperty<TCtl, TProp>(this TCtl control, Expression<Func<TProp>> propexpr) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (propexpr == null)
                throw new ArgumentNullException(nameof(propexpr));

            // get the control's dispatcher
            var disp = control.Dispatcher;

            // check if dispatching is required
            // if it is, perform it
            // this will invoke this method from (hopefully) the control's thread
            if (!disp.CheckAccess())
                return (TProp)disp.Invoke(new GetPropertyDelegate<TCtl, TProp>(GetProperty), control, propexpr);

            // get the body of the expression, check if it's an expression that 
            // results in a class member being passed
            if (!(propexpr.Body is MemberExpression propexprm))
                throw new ArgumentException("Invalid member expression.", nameof(propexpr));

            // get the member from the expression body, and check if it's a property
            var prop = propexprm.Member as PropertyInfo;
            if (prop == null)
                throw new ArgumentException("Invalid property supplied.", nameof(propexpr));

            // finally, set the value of the property to the supplied one
            return (TProp)prop.GetValue(control);
        }

        // this method invokes a return-less method for given control
        // usage is control.InvokeAction(new Action<T1, T2, ...>(method), arg1, arg2, ...)
        public static void InvokeAction<TCtl>(this TCtl control, Delegate dlg, params object[] args) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (dlg == null)
                throw new ArgumentNullException(nameof(dlg));

            // get the control's dispatcher
            var disp = control.Dispatcher;

            // check if dispatching is required
            if (!disp.CheckAccess())
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                disp.Invoke(new InvokeActionDelegate<TCtl>(InvokeAction), control, dlg, args);
                return;
            }

            // finally, call the passed delegate, with supplied arguments
            dlg.DynamicInvoke(args);
        }

        // this method invokes a method which returns for given control, the returned value is returned to the caller
        // usage is control.InvokeAction<TReturn>(new Func<T1, T2, ..., TReturn>(method), arg1, arg2, ...)
        public static TResult InvokeFunc<TCtl, TResult>(this TCtl control, Delegate dlg, params object[] args) where TCtl : Control
        {
            // check for nulls
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (dlg == null)
                throw new ArgumentNullException(nameof(dlg));

            // get the control's dispatcher
            var disp = control.Dispatcher;

            // check if dispatching is required
            if (!disp.CheckAccess())
            {
                // if it is, perform it
                // this will invoke this method from (hopefully) the control's thread
                return (TResult)disp.Invoke(new InvokeFuncDelegate<TCtl, TResult>(InvokeFunc<TCtl, TResult>), control, dlg, args);
            }

            // finally, call the passed delegate, with supplied arguments and return 
            // the result
            return (TResult)dlg.DynamicInvoke(args);
        }
    }
}