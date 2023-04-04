# MauiNumberAnimation
Control creado en dotNetMaui para mostrar un número con una animación mientras se in/decrementa su valor.

![Aplicación de ejemplo creada](https://jorgediegocrespo.files.wordpress.com/2023/03/numberanimation.gif?w=362) 

El control recibe un valor, un *string format* y el tiempo, en milisegundos, que debe durar la animación. 

Si inspeccionamos el control, vemos que tiene una *bindable property* por cada uno de los valores expuestos.

```csharp
public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(decimal), typeof(AnimatedValue), defaultValue: 0m, defaultBindingMode: BindingMode.OneWay, propertyChanged: ValueChanged);
public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(AnimatedValue), defaultBindingMode: BindingMode.OneWay, propertyChanged: FormatChanged);
public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimatedValue), defaultValue: (uint)1_000, defaultBindingMode: BindingMode.OneWay);
...
public decimal Value
{
    get => (decimal)GetValue(ValueProperty);
    set => SetValue(ValueProperty, value);
}

public string Format
{
    get => (string)GetValue(FormatProperty);
    set => SetValue(FormatProperty, value);
}

public uint Duration
{
    get => (uint)GetValue(DurationProperty);
    set => SetValue(DurationProperty, value);
}
```

La interfaz de usuario de este control también es bien sencilla, ya que solamente tiene un *label*, que utilizaremos para mostrar el valor en su formato correspondiente.

```xml
<ContentView xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="NumberAnimation.AnimatedValue">
    <Label 
        x:Name="lbNumber"
        FontSize="32"
        VerticalOptions="Center" 
        HorizontalOptions="Center" />
</ContentView>
```

Por último, hay que destacar el funcionamiento del control. Siempre que cambia el valor, se lanza una animación, cancelandose previamente la anterior, por si todavía se está ejecutando. 

```csharp
private const string ANIMATION_NAME = "NumberAnimation";

private void ExecuteAnimation(double start, double end)
{
    this.AbortAnimation(ANIMATION_NAME);
    Animation animation = new Animation(x => lbNumber.Text = x.ToString(Format), start, end, null, () => lbNumber.Text = end.ToString(Format));
    animation.Commit(this, ANIMATION_NAME, 16, Duration);
}

private static void ValueChanged(BindableObject bindable, object oldValue, object newValue)
{
    double start = Decimal.ToDouble((decimal)oldValue);
    double end = Decimal.ToDouble((decimal)newValue);
    ((AnimatedValue)bindable).ExecuteAnimation(start, end);
}
```

Del mismo modo, si cambia el formato, se actualiza el texto del *label* para reflejarlo.

```csharp
private static void FormatChanged(BindableObject bindable, object oldValue, object newValue)
{
    ((AnimatedValue)bindable).lbNumber.Text = ((AnimatedValue)bindable).Value.ToString((string)newValue);
}
```

Notese, que si cambia la duranción de la animación no se hace nada, ya que se tendrá en cuenta al ejecutar la siguiente animación.
