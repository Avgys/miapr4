using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PerceptronNS
{
    public class Perceptron
    {
        public class PerceptronClass
        {
            public readonly List<PerceptronObject> Objects = new List<PerceptronObject>();
            public readonly PerceptronObject Weight;
        }

        public class PerceptronObject
        {
            public readonly List<int> Attributes = new List<int>();
        }

        private const int C = 1;
        private const int Random = 10;
        private const int MaxIterationsCount = 10000;

        private readonly int _classesCount;
        private readonly int _objectsInClassCount;
        private readonly int _attributesCount;
        public List<PerceptronClass> _classes = new List<PerceptronClass>();
        public List<PerceptronObject> _weights = new List<PerceptronObject>();
        public List<PerceptronObject> _decisionFunctions;
        private readonly List<int> _decisions = new List<int>();

        public Perceptron(int classes, int objectsInClass, int attribute)
        {
            _classesCount = classes;
            _objectsInClassCount = objectsInClass;
            _attributesCount = attribute;
        }

        public List<PerceptronClass> Calculate()
        {
            CreateRandomClasses();
            CalculateDecisionFunctions();
            return _classes;
        }

        private void CreateRandomClasses()
        {
            var rand = new Random();

            for (var i = 0; i < _classesCount; i++)
            {
                var currentClass = new PerceptronClass();

                for (var j = 0; j < _objectsInClassCount; j++)
                {
                    var currentObject = new PerceptronObject();

                    for (var k = 0; k < _attributesCount; k++)
                        currentObject.Attributes.Add(rand.Next(Random) - Random / 2);

                    currentClass.Objects.Add(currentObject);
                }
                _classes.Add(currentClass);
            }

            foreach (var weight in _classes.Select(perceptronClass => new PerceptronObject()))
            {
                for (var i = 0; i <= _attributesCount; i++)
                    weight.Attributes.Add(0);

                _weights.Add(weight);
                _decisions.Add(0);
            }

            foreach (var perceptronObject in _classes.SelectMany(perceptronClass => perceptronClass.Objects))
                perceptronObject.Attributes.Add(1);
        }

        private void CalculateDecisionFunctions()
        {
            var IsClassification = true;
            var iteration = 0;

            while (IsClassification && (iteration < MaxIterationsCount))
            {
                for (var i = 0; i < _classes.Count; i++)
                {
                    var currentClass = _classes[i];
                    var currentWeight = _weights[i];

                    foreach (var currentObject in currentClass.Objects)
                    {
                        IsClassification = CorrectWeight(currentObject, currentWeight, i);
                    }
                }
                iteration++;
            }

            if (iteration == MaxIterationsCount)
                MessageBox.Show($"Количество итераций превысило {MaxIterationsCount}.{Environment.NewLine}Решаюшие функции, возможно, найдены неправильно.");

            _decisionFunctions = _weights;
        }

        private bool CorrectWeight(PerceptronObject currentObject,
            PerceptronObject currentWeight, int classNumber)
        {
            var result = false;
            var objectDecision = ObjectMultiplication(currentWeight, currentObject);

            for (var i = 0; i < _weights.Count; i++)
            {
                _decisions[i] = ObjectMultiplication(_weights[i], currentObject);

                if (i == classNumber) continue;
                var currentDecision = _decisions[i];
                if (objectDecision > currentDecision) continue;
                ChangeWeight(_weights[i], currentObject, -1);

                result = true;
            }
            if (result)
                ChangeWeight(currentWeight, currentObject, 1);

            return result;
        }

        private static int ObjectMultiplication(PerceptronObject weight, PerceptronObject obj)
        {
            return weight.Attributes.Select((t, i) => t * obj.Attributes[i]).Sum();
        }

        private static void ChangeWeight(PerceptronObject weight, PerceptronObject perceptronObject, int sign)
        {
            for (var i = 0; i < weight.Attributes.Count; i++)
            {
                weight.Attributes[i] += sign * perceptronObject.Attributes[i];
            }
        }

        public int FindClass(PerceptronObject perceptronObject)
        {
            var resultClass = 0;

            perceptronObject.Attributes.Add(1);
            var decisionMax = ObjectMultiplication(_weights[0], perceptronObject);

            for (var i = 1; i < _weights.Count; i++)
            {
                var weight = _weights[i];

                if (ObjectMultiplication(weight, perceptronObject) <= decisionMax) continue;
                decisionMax = ObjectMultiplication(weight, perceptronObject);
                resultClass = i;
            }

            return (resultClass + 1);
        }
    }
}