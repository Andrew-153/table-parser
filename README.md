# Парсер полей 
> [(учебный проект)](https://ulearn.me/course/basicprogramming/Praktika_Polya_v_kavychkakh__7a098f71-f436-436f-92ed-287d7b1bca3c)
> реализован в рамкам практических задач на платформе [Ulearn.me](ulearn.me)

### Описание проекта

 В данном проекте реализован алгоритм, возвращающий список полей, извлечённых из текста  
 по предопредленным правилам, либо пустой список, если полей в исходном тексте не оказалось.


Проверить работу парсера можно запустив программу. Появится окно, в котором можно   
вводить разные входные строки и смотреть, на какие поля эти строки разбиваются.

#### Типы полей

В этом проекте аллгоритм распознает и отображает несколько типов полей: простые и в кавычках.  
На вход нашего парсера подается строка текста, а алгоритм проходит по нему и разделяет на подстроки  
заключенные в кавычки или без них.


#### Пример работы алгоритма
Если ввести некоторую строку: `some_text "QF \"" other_text`  
То резульатом будет три строки:  
* `some_text` Position=0 Length=9
* `QF "`      Position=10 Length=7
* `other_text`  Position=18 Length=10

### Проделанная работа

В этом проекте я реализовал несколько классов и покрыл их модульными тестами.    
1. Первый класс - `FieldsParserTask.cs` - реализован метод чтения и формирование подстрок
   по описанным выше правилам.  
   В этом классе имеется Unit тесты для проверки корректности работы программы.

2. Второй класс `QuotedFieldTask.cs` - реализован метод для чтения полей в кавычках. 
   В этом классе также учитывается, что строка может быть интерпретирована как `дословный строковый литерал`.   
   Поэтому учитывается экранирование кавычек `'`, `"` и знака слэш `\`.
   В этом классе также имеется Unit тесты для проверки корректности работы данного метода.


