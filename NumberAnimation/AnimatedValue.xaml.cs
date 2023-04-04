namespace NumberAnimation;

public partial class AnimatedValue : ContentView
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(decimal), typeof(AnimatedValue), defaultValue: 0m, defaultBindingMode: BindingMode.OneWay, propertyChanged: ValueChanged);
    public static readonly BindableProperty FormatProperty = BindableProperty.Create(nameof(Format), typeof(string), typeof(AnimatedValue), defaultBindingMode: BindingMode.OneWay, propertyChanged: FormatChanged);
    public static readonly BindableProperty DurationProperty = BindableProperty.Create(nameof(Duration), typeof(uint), typeof(AnimatedValue), defaultValue: (uint)1_000, defaultBindingMode: BindingMode.OneWay);

    private const string ANIMATION_NAME = "NumberAnimation";

    public AnimatedValue()
    {
		InitializeComponent();
        lbNumber.Text = "0";
	}

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

    private static void FormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((AnimatedValue)bindable).lbNumber.Text = ((AnimatedValue)bindable).Value.ToString((string)newValue);
    }
}