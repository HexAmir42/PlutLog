using System;
using System.Collections.Generic;
using System.Text;

namespace PlutLogRe.Models
{
    public class Model
    {
        public PlutDocument Document { get; private set; }
        public Dictionary<string, string> SysPredicatesDescriptions = new Dictionary<string, string>()
        {
            { "_planet", "Возвращает планеты Солнечной системы" },
            { "_write", "Осуществляет вывод значения" },
            { "_nl", "Перевод строки" },
            { "_is", "Предикат унификации" },
            { "_if", "Предикат условия" },
            { "_not", "Предикат отрицания" },
            { "_cut", "Отсечение" },
            { "_fail", "Возврат неуспеха" },
            { "_read", "Пользовательский ввод" },
            { "_add", "_add(X,Y) ~ X+Y" },
            { "_sub", "_sub(X,Y) ~ X-Y" },
            { "_mul", "_mul(X,Y) ~ X*Y" },
            { "_div", "_div(X,Y) ~ X/Y" },
            { "_mod", "_mod(X,Y) ~ X%Y" },
            { "_gt", "_gt(X,Y) ~ X>Y" },
            { "_gte", "_gte(X,Y) ~ X>=Y" },
            { "_eq", "_eq(X,Y) ~ X==Y" },
            { "_neq", "_neq(X,Y) ~ X!=Y" },
            { "_ls", "_ls(X,Y) ~ X<Y" },
            { "_lse", "_lse(X,Y) ~ X<=Y" },
            { "_lnot", "Логическое отрицание" },
            { "_and", "Логическое И" },
            { "_or", "Логическое ИЛИ" },
            { "_xor", "Логическое исключающее ИЛИ" },
            { "_impl", "Логическая имплементация" },
            { "_abs", "Возвращает модуль числа" },
            { "_rnd", "Возвращает рандомное число" },
            { "_pow", "Возводит число в степень" },
            { "_min", "Возвращает минимальное из двух чисел" },
            { "_max", "Возвращает максимальное из двух чисел" },
        };

        public void OpenDocument(string path)
        {
            Document = new PlutDocument(path);
        }

        public void SaveDocument(string path, string content)
        {
            Document = new PlutDocument(path, content);
        }
    }
}
