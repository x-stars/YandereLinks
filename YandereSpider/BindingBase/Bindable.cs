using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// <see cref="INotifyPropertyChanged"/> 接口的实现，用于实现数据绑定到用户控件的泛型类。
    /// </summary>
    /// <remarks><para>
    /// 设置数据绑定时，请绑定到 <see cref="Bindable{T}.Value"/>，而非实例本身，以实现属性值发生更改时通知客户端。
    /// </para><para>
    /// 除初始化外，请不要直接给此类的实例赋值，而应更改 <see cref="Bindable{T}.Value"/> 的值。
    /// </para><para>
    /// 直接更改实例的值将不会触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件，
    /// 并会替换 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托，破坏绑定关系。
    /// </para></remarks>
    /// <typeparam name="T"><see cref="Bindable{T}"/> 中用于数据绑定的值的类型。</typeparam>
    public class Bindable<T> : BindableObject, IEquatable<Bindable<T>>
    {
        /// <summary>
        /// 当前 <see cref="Bindable{T}"/> 实例用于数据绑定的值。
        /// </summary>
        private T value;

        /// <summary>
        /// 使用默认值初始化 <see cref="Bindable{T}"/> 类的新实例。
        /// </summary>
        public Bindable() : base() => this.value = default(T);

        /// <summary>
        /// 使用指定的值初始化 <see cref="Bindable{T}"/> 类的新实例。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的对象。</param>
        public Bindable(T value) : base() => this.value = value;

        /// <summary>
        /// 当前 <see cref="Bindable{T}"/> 实例用于数据绑定的值。
        /// </summary>
        /// <remarks>
        /// 设定绑定时，应绑定到此属性，而非实例本身。
        /// 更改 <see cref="Bindable{T}"/> 实例的值时，也应更改此属性的值，而非实例本身。
        /// </remarks>
        public T Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="Bindable{T}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="Bindable{T}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的 <see cref="Bindable{T}.Value"/> 属性相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(Bindable<T> other) =>
            !(other is null) && EqualityComparer<T>.Default.Equals(this.value, other.value);

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="Bindable{T}"/> 的实例，
        /// 且 <see cref="Bindable{T}.Value"/> 属性相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj) =>
            this.Equals(obj as Bindable<T>);

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode() =>
            -1584136870 + EqualityComparer<T>.Default.GetHashCode(this.value);

        /// <summary>
        /// 返回表示当前实例的值的字符串。
        /// </summary>
        /// <returns><see cref="Bindable{T}.Value"/> 的等效字符串表达形式。</returns>
        public override string ToString() => this.value.ToString();

        /// <summary>
        /// 指示两 <see cref="Bindable{T}"/> 对象是否相等。
        /// </summary>
        /// <param name="bindable1">第一个对象。</param>
        /// <param name="bindable2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="bindable1"/> 的 <see cref="Bindable{T}.Value"/>
        /// 等于 <paramref name="bindable2"/> 的 <see cref="Bindable{T}.Value"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(Bindable<T> bindable1, Bindable<T> bindable2) =>
            EqualityComparer<Bindable<T>>.Default.Equals(bindable1, bindable2);

        /// <summary>
        /// 指示两 <see cref="Bindable{T}"/> 对象是否不等。
        /// </summary>
        /// <param name="bindable1">第一个对象。</param>
        /// <param name="bindable2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="bindable1"/> 的 <see cref="Bindable{T}.Value"/>
        /// 不等于 <paramref name="bindable2"/> 的 <see cref="Bindable{T}.Value"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(Bindable<T> bindable1, Bindable<T> bindable2) =>
            !(bindable1 == bindable2);

        /// <summary>
        /// 创建一个新的 <see cref="Bindable{T}"/> 对象，并将其用于数据绑定的值初始化为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的对象。</param>
        /// <returns>使用 <paramref name="value"/> 初始化的 <see cref="Bindable{T}"/> 对象。</returns>
        public static implicit operator Bindable<T>(T value) => new Bindable<T>(value);

        /// <summary>
        /// 返回指定 <see cref="Bindable{T}"/> 对象用于数据绑定的值。
        /// </summary>
        /// <param name="bindable">一个 <see cref="Bindable{T}"/> 对象。</param>
        /// <returns><paramref name="bindable"/> 用于数据绑定的值。</returns>
        public static implicit operator T(Bindable<T> bindable) => bindable.value;
    }
}
