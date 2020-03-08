using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="ObservableObject"/> 类的新实例。
        /// </summary>
        protected ObservableObject() { }

        /// <summary>
        /// 在属性值更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 设置指定属性的值，并通知属性值已更改。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="field">属性对应的字段。</param>
        /// <param name="value">属性的新值，一般为 <see langword="value"/>。</param>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        protected void SetProperty<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            field = value;
            this.NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// 通知指定属性值已更改。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称，可由编译器自动获取。</param>
        protected void NotifyPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ObservableObject.PropertyChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="PropertyChangedEventArgs"/>。</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }
}
