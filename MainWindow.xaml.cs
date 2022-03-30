using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PerceptronNS;
using static PerceptronNS.Perceptron;

namespace miapr5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Perceptron _perceptron;
        //private List<TextBox> _textBlocksDictionary = new List<TextBox>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClassify_Click(object sender, RoutedEventArgs e)
        {
            var testObject = new PerceptronObject();
            //var numbers = new int[_textBlocksDictionary.Count];
            var regex = new Regex("(,-?[0-9]+$)");
            var str = InputObject.Text;
            str.Replace(" ", "");
            
            

            try
            {
                if (!regex.IsMatch(str))
                {
                    throw new ArgumentException($"Invalid input symbols");
                }

                var numbers = str.Split(',');
                if (numbers.Length != _perceptron._attributesCount)
                {
                    throw new ArgumentException($"Not setted enough attributes {numbers.Length} / {_perceptron._attributesCount}");
                }

                for (var i = 0; i < _perceptron._attributesCount; i++)
                {
                    testObject.Attributes.Add(int.Parse(numbers[i]));
                }

                MessageBox.Show($"Объект относится к {_perceptron.FindClass(testObject)} классу");
            }
            catch (ArgumentException exception)
            {
                MessageBox.Show(exception.Message);
            }
            catch
            {
                MessageBox.Show("Ошибка ввода тестовой сборки");
            }
        }

        private void ButtonCreateSelection_Click(object sender, RoutedEventArgs e)
        {
            //CreatingElementsTool.MakeInputBoxes((int)SliderAttributeNumber.Value, inputPanel, _textBlocksDictionary, TextCheck);
            ListViewSolutions.Items.Clear();
            ListViewSelection.Items.Clear();
            _perceptron = new Perceptron((int)SliderClassesNumber.Value, (int)SliderObjectNumber.Value,
                (int)SliderAttributeNumber.Value);
            _perceptron.Calculate();
            FillListViewSelection(ListViewSelection, _perceptron._classes);
            FillListViewSolution(ListViewSolutions, _perceptron._decisionFunctions);
        }

        private void FillListViewSolution(ListView ListViewSelection, List<PerceptronObject> _decisionFunctions)
        {
            for (var i = 0; i < _decisionFunctions.Count; i++)
            {
                var str = $"d{i + 1}(x) = ";

                for (var j = 0; j < _decisionFunctions[i].Attributes.Count; j++)
                {
                    var attribute = _decisionFunctions[i].Attributes[j];

                    if (j < _decisionFunctions[i].Attributes.Count - 1)
                        if (attribute >= 0 && j != 0)
                            str += "+" + attribute + $"*x{j + 1}";
                        else
                            str += attribute + $"*x{j + 1}";
                    else
                        if (attribute >= 0 && j != 0)
                        str += "+" + attribute;
                    else
                        str += attribute;
                }
                ListViewSelection.Items.Add(str);
            }
        }

        private void FillListViewSelection(ListView ListViewSelection, List<PerceptronClass> classes)
        {
            var indexCurrentClass = 1;
            foreach (var currentClass in classes)
            {
                int indexCurrentObject = 1;
                ListViewSelection.Items.Add($"Класс {indexCurrentClass}");


                foreach (var currentObject in currentClass.Objects)
                {
                    var str = $"\tОбъект {indexCurrentObject}: (";

                    for (var j = 0; j < currentObject.Attributes.Count - 1; j++)
                    {
                        var attribute = currentObject.Attributes[j];
                        str += attribute + ",";
                    }
                    str = str.Remove(str.Length - 1, 1);
                    str += ")";

                    ListViewSelection.Items.Add(str);
                    indexCurrentObject++;
                }
                indexCurrentClass++;
            }
        }

        private void TextCheck(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("(^-?[0-9]+$)|(^-?$)");
            if (e.Text == " ") e.Handled = true;
            if (!regex.IsMatch($"{(sender as TextBox)?.Text}{e.Text}")) e.Handled = true;

        }

        //public void FillListViews(ListView listViewSelection, ListView listViewSolutions)
        //{
        //    var indexCurrentClass = 1;

        //    foreach (var currentClass in _classes)
        //    {
        //        var indexCurrentObject = 1;
        //        CreatingElementsTool.CreateTextBlockOnListView($"Класс {indexCurrentClass}:",
        //            listViewSelection);
        //        foreach (var currentObject in currentClass.Objects)
        //        {
        //            var str = $"\tОбъект {indexCurrentObject}: (";

        //            for (var j = 0; j < currentObject.Attributes.Count - 1; j++)
        //            {
        //                var attribute = currentObject.Attributes[j];
        //                str += attribute + ",";
        //            }
        //            str = str.Remove(str.Length - 1, 1);
        //            str += ")";
        //            CreatingElementsTool.CreateTextBlockOnListView(str, listViewSelection);
        //            indexCurrentObject++;
        //        }
        //        CreatingElementsTool.CreateTextBlockOnListView("", listViewSelection);
        //        indexCurrentClass++;
        //    }

        //    for (var i = 0; i < _decisionFunctions.Count; i++)
        //    {
        //        var str = $"d{i + 1}(x) = ";

        //        for (var j = 0; j < _decisionFunctions[i].Attributes.Count; j++)
        //        {
        //            var attribute = _decisionFunctions[i].Attributes[j];

        //            if (j < _decisionFunctions[i].Attributes.Count - 1)
        //                if (attribute >= 0 && j != 0)
        //                    str += "+" + attribute + $"*x{j + 1}";
        //                else
        //                    str += attribute + $"*x{j + 1}";
        //            else
        //                if (attribute >= 0 && j != 0)
        //                str += "+" + attribute;
        //            else
        //                str += attribute;
        //        }
        //        CreatingElementsTool.CreateTextBlockOnListView(str, listViewSolutions);
        //    }
        //}
    }
}