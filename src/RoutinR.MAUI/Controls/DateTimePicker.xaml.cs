namespace RoutinR.MAUI.Controls;

public partial class DateTimePicker : ContentView
{
    public static readonly BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimePicker), default(DateTime));

    public DateTime DateTime
    {
        get => (DateTime)GetValue(DateTimePicker.DateTimeProperty);
        set => SetValue(DateTimePicker.DateTimeProperty, value);
    }

    public DateTimePicker()
	{
		InitializeComponent();
	}

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        TimePickerControl.Time = DateTime.TimeOfDay;
        DatePickerControl.Date = DateTime.Date;
    }

    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        var timeOfDay = DateTime.TimeOfDay;
        DateTime = e.NewDate.Add(timeOfDay);
    }

    private void TimePicker_Unfocused(object sender, FocusEventArgs e)
    {
        var timeOfDay = TimePickerControl.Time;
        var oldDate = DateTime.Date;

        DateTime = oldDate.Add(timeOfDay);
    }
}