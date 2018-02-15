﻿using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace KegID.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<ListView>
    {
        Delegate eventHandler;
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
                }
                MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
                eventInfo.AddEventHandler(AssociatedObject, eventHandler);
            }
            catch (Exception)
            {

            }

        }

        void DeregisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                if (eventHandler == null)
                {
                    return;
                }
                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
                }
                eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
                eventHandler = null;
            }
            catch (Exception)
            {

            }
        }

        void OnEvent(object sender, object eventArgs)
        {
            try
            {
                if (Command == null)
                {
                    return;
                }

                object resolvedParameter;
                if (CommandParameter != null)
                {
                    resolvedParameter = CommandParameter;
                }
                else if (Converter != null)
                {
                    resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
                }
                //else if (eventArgs is SelectedItemChangedEventArgs)
                //{
                //    resolvedParameter =
                //        ((SelectedItemChangedEventArgs)eventArgs)
                //          .SelectedItem;
                //}
                //else if (eventArgs is ItemTappedEventArgs)
                //{
                //    resolvedParameter =
                //        ((ItemTappedEventArgs)eventArgs)
                //          .Item;
                //}
                //else if (eventArgs is PropertyChangedEventArgs)
                //{
                //    resolvedParameter = sender;
                //}
                //else if (eventArgs is FocusEventArgs)
                //{
                //    resolvedParameter = sender;
                //}
                //else if (eventArgs is EventArgs)
                //{
                //    resolvedParameter = sender;
                //}
                else
                {
                    resolvedParameter = eventArgs;
                }


                if (Command.CanExecute(resolvedParameter))
                {
                    Command.Execute(resolvedParameter);
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }

        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }

    public class EntryEventToCommandBehavior : BehaviorBase<Entry>
    {
        Delegate eventHandler;
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EntryEventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EntryEventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EntryEventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EntryEventToCommandBehavior), null);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
                }
                MethodInfo methodInfo = typeof(EntryEventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
                eventInfo.AddEventHandler(AssociatedObject, eventHandler);
            }
            catch (Exception)
            {

            }
        }

        void DeregisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                if (eventHandler == null)
                {
                    return;
                }
                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
                }
                eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
                eventHandler = null;
            }
            catch (Exception)
            {

            }
        }

        void OnEvent(object sender, object eventArgs)
        {
            try
            {
                if (Command == null)
                {
                    return;
                }

                object resolvedParameter;
                if (CommandParameter != null)
                {
                    resolvedParameter = CommandParameter;
                }
                else if (Converter != null)
                {
                    resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
                }
                else
                {
                    resolvedParameter = eventArgs;
                }

                if (Command.CanExecute(resolvedParameter))
                {
                    Command.Execute(resolvedParameter);
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EntryEventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }

    public class ButtonEventToCommandBehavior : BehaviorBase<Button>
    {
        Delegate eventHandler;
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(ButtonEventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ButtonEventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(ButtonEventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(ButtonEventToCommandBehavior), null);

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        protected override void OnAttachedTo(Button bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(Button bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        void RegisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));
                }
                MethodInfo methodInfo = typeof(ButtonEventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
                eventInfo.AddEventHandler(AssociatedObject, eventHandler);
            }
            catch (Exception)
            {

            }
        }

        void DeregisterEvent(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return;
                }

                if (eventHandler == null)
                {
                    return;
                }
                EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);
                if (eventInfo == null)
                {
                    throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));
                }
                eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
                eventHandler = null;
            }
            catch (Exception)
            {

            }
        }

        void OnEvent(object sender, object eventArgs)
        {
            try
            {
                if (Command == null)
                {
                    return;
                }

                object resolvedParameter;
                if (CommandParameter != null)
                {
                    resolvedParameter = CommandParameter;
                }
                else if (Converter != null)
                {
                    resolvedParameter = Converter.Convert(eventArgs, typeof(object), null, null);
                }
                else
                {
                    resolvedParameter = eventArgs;
                }

                if (Command.CanExecute(resolvedParameter))
                {
                    Command.Execute(resolvedParameter);
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
        }

        static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (ButtonEventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }

}
