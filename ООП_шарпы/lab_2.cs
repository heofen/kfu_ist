using System.Collections;

namespace lab_2
{
    public enum Education
    {
        Specialist,
        Bachelor,
        SecondEducation
    }

    interface IDateAndCopy
    {
        object DeepCopy();
        DateTime Date { get; set; }
    }

    public class Person : IDateAndCopy
    {
        protected string firstName;
        protected string lastName;
        protected DateTime birthDate;

        public Person(string firstName, string lastName, DateTime birthDate)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        public Person()
        {
            this.firstName = "None";
            this.lastName = "None";
            this.birthDate = DateTime.MinValue;
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        public int BirthYear
        {
            get { return birthDate.Year; }
            set { birthDate = new DateTime(value, birthDate.Month, birthDate.Day); }
        }

        public override string ToString()
        {
            return $"Name: {firstName}, Last name: {lastName}, BirthDate: {birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return $"{firstName} {lastName}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Person other = (Person)obj;
            return firstName == other.firstName && lastName == other.lastName && birthDate == other.birthDate;
        }

        public static bool operator ==(Person person1, Person person2)
        {
            if (ReferenceEquals(person1, person2))
            {
                return true;
            }
            if (person1 is null || person2 is null)
            {
                return false;
            }
            return person1.Equals(person2);
        }

        public static bool operator !=(Person person1, Person person2)
        {
            return !(person1 == person2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(firstName, lastName, birthDate);
        }

        public virtual object DeepCopy()
        {
            return new Person(firstName, lastName, birthDate);
        }

        DateTime IDateAndCopy.Date
        {
            get { return BirthDate; }
            set { BirthDate = value; }
        }
    }

    public class Exam : IDateAndCopy
    {
        public string Subject { get; set; }
        public int Grade { get; set; }
        public DateTime Date { get; set; }

        public Exam(string subject, int grade, DateTime date)
        {
            Subject = subject;
            Grade = grade;
            Date = date;
        }

        public Exam()
        {
            Subject = "None";
            Grade = 0;
            Date = DateTime.MinValue;
        }

        public override string ToString()
        {
            return $"Subject: {Subject}, Grade: {Grade}, Date: {Date.ToShortDateString()}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Exam other = (Exam)obj;
            return Subject == other.Subject && Grade == other.Grade && Date == other.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Subject, Grade, Date);
        }

        public static bool operator ==(Exam exam1, Exam exam2)
        {
            if (ReferenceEquals(exam1, exam2))
            {
                return true;
            }
            if (exam1 is null || exam2 is null)
            {
                return false;
            }
            return exam1.Equals(exam2);
        }

        public static bool operator !=(Exam exam1, Exam exam2)
        {
            return !(exam1 == exam2);
        }

        public object DeepCopy()
        {
            return new Exam(Subject, Grade, Date);
        }
    }

    public class Test : IDateAndCopy
    {
        public string Subject { get; set; }
        public bool IsPassed { get; set; }
        public DateTime Date { get; set; }

        public Test(string subject, bool isPassed, DateTime date)
        {
            Subject = subject;
            IsPassed = isPassed;
            Date = date;
        }

        public Test() : this("Default Subject", false, DateTime.Now) { }

        public override string ToString()
        {
            return $"Test: {Subject}, Passed: {IsPassed}, Date: {Date.ToShortDateString()}";
        }

        public object DeepCopy()
        {
            return new Test(Subject, IsPassed, Date);
        }
    }

    public class StudentEnumerator : IEnumerator<string>
    {
        private IEnumerator _testEnumerator;
        private IEnumerator _examEnumerator;
        private HashSet<string> _testSubjects;
        private HashSet<string> _examSubjects;
        private string _current;

        public StudentEnumerator(ArrayList tests, ArrayList exams)
        {
            _testEnumerator = tests.GetEnumerator();
            _examEnumerator = exams.GetEnumerator();
            _testSubjects = new HashSet<string>();
            foreach (Test test in tests)
            {
                _testSubjects.Add(test.Subject);
            }
            _examSubjects = new HashSet<string>();
            foreach (Exam exam in exams)
            {
                _examSubjects.Add(exam.Subject);
            }
            Reset();
        }

        public string Current => _current;

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            while (_testEnumerator.MoveNext())
            {
                Test currentTest = (Test)_testEnumerator.Current;
                if (_examSubjects.Contains(currentTest.Subject))
                {
                    _current = currentTest.Subject;
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            _testEnumerator.Reset();
            _examEnumerator.Reset();
            _current = null;
        }
    }

    public class Student : Person, IDateAndCopy, IEnumerable
    {
        private Education education;
        private int groupNumber;
        private ArrayList tests = new ArrayList();
        private ArrayList exams = new ArrayList();

        public Student(Person person, Education education, int groupNumber) : base(person.FirstName, person.LastName, person.BirthDate)
        {
            this.education = education;
            this.groupNumber = groupNumber;
        }

        public Student() : base()
        {
            this.education = Education.Bachelor;
            this.groupNumber = 101;
        }

        public Person PersonInfo
        {
            get { return new Person(FirstName, LastName, BirthDate); }
            set
            {
                FirstName = value.FirstName;
                LastName = value.LastName;
                BirthDate = value.BirthDate;
            }
        }

        public Education EducationForm
        {
            get { return education; }
            set { education = value; }
        }

        public int GroupNumber
        {
            get { return groupNumber; }
            set
            {
                if (value <= 100 || value > 599)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Номер группы должен быть в диапазоне от 101 до 599.");
                }
                groupNumber = value;
            }
        }

        public ArrayList Exams
        {
            get { return exams; }
            set { exams = value; }
        }

        public double AverageGrade
        {
            get
            {
                if (exams.Count == 0) return 0.0;
                double sum = 0;
                foreach (Exam exam in exams)
                {
                    sum += exam.Grade;
                }
                return sum / exams.Count;
            }
        }

        public void AddExams(params Exam[] newExams)
        {
            exams.AddRange(newExams);
        }

        public void AddTests(params Test[] newTests)
        {
            tests.AddRange(newTests);
        }

        public override string ToString()
        {
            string examList = string.Join("\n", exams.ToArray());
            string testList = string.Join("\n", tests.ToArray());
            return $"Student: {base.ToString()}, Education: {education}, Group: {groupNumber}\nExams:\n{examList}\nTests:\n{testList}";
        }

        public override string ToShortString()
        {
            return $"Student: {base.ToShortString()}, Education: {education}, Group: {groupNumber}, Average Grade: {AverageGrade:F2}";
        }

        public override object DeepCopy()
        {
            Student newStudent = new Student((Person)base.DeepCopy(), education, groupNumber);
            newStudent.exams = new ArrayList();
            foreach (Exam exam in exams)
            {
                newStudent.exams.Add(exam.DeepCopy());
            }
            newStudent.tests = new ArrayList();
            foreach (Test test in tests)
            {
                newStudent.tests.Add(test.DeepCopy());
            }
            return newStudent;
        }

        DateTime IDateAndCopy.Date
        {
            get { return BirthDate; }
            set { BirthDate = value; }
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj) || GetType() != obj.GetType())
            {
                return false;
            }

            Student other = (Student)obj;
            if (education != other.education || groupNumber != other.groupNumber || tests.Count != other.tests.Count || exams.Count != other.exams.Count)
            {
                return false;
            }

            for (int i = 0; i < tests.Count; i++)
            {
                if (!tests[i].Equals(other.tests[i]))
                {
                    return false;
                }
            }

            for (int i = 0; i < exams.Count; i++)
            {
                if (!exams[i].Equals(other.exams[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            hashCode = HashCode.Combine(hashCode, education, groupNumber);
            foreach (var test in tests)
            {
                hashCode = HashCode.Combine(hashCode, test);
            }
            foreach (var exam in exams)
            {
                hashCode = HashCode.Combine(hashCode, exam);
            }
            return hashCode;
        }

        public static bool operator ==(Student student1, Student student2)
        {
            if (ReferenceEquals(student1, student2))
            {
                return true;
            }
            if (student1 is null || student2 is null)
            {
                return false;
            }
            return student1.Equals(student2);
        }

        public static bool operator !=(Student student1, Student student2)
        {
            return !(student1 == student2);
        }

                public IEnumerable GetAllTestsAndExams()
        {
            foreach (var test in tests)
            {
                yield return test;
            }
            foreach (var exam in exams)
            {
                yield return exam;
            }
        }

                public IEnumerable GetExamsAboveGrade(int minGrade)
        {
            foreach (Exam exam in exams)
            {
                if (exam.Grade > minGrade)
                {
                    yield return exam;
                }
            }
        }

                                IEnumerator IEnumerable.GetEnumerator()
        {
            return new StudentEnumerator(tests, exams);
        }

                        public IEnumerable GetPassedTestsAndExams()
        {
            foreach (Test test in tests)
            {
                if (test.IsPassed)
                {
                    yield return test;
                }
            }
            foreach (Exam exam in exams)
            {
                if (exam.Grade > 2)
                {
                    yield return exam;
                }
            }
        }

        public IEnumerable GetPassedTestsWithPassedExams()
        {
            var passedExamSubjects = new HashSet<string>();
            foreach (Exam exam in exams)
            {
                if (exam.Grade > 2)
                {
                    passedExamSubjects.Add(exam.Subject);
                }
            }

            foreach (Test test in tests)
            {
                if (test.IsPassed && passedExamSubjects.Contains(test.Subject))
                {
                    yield return test;
                }
            }
        }
    }

    public class lab_2
    {
        public static void Main(string[] args)
        {
                        Person person1 = new Person("John", "Doe", new DateTime(1990, 1, 1));
            Person person2 = new Person("John", "Doe", new DateTime(1990, 1, 1));

            Console.WriteLine($"ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");
            Console.WriteLine($"person1.Equals(person2): {person1.Equals(person2)}");
            Console.WriteLine($"person1 == person2: {person1 == person2}");
            Console.WriteLine($"person1.GetHashCode(): {person1.GetHashCode()}");
            Console.WriteLine($"person2.GetHashCode(): {person2.GetHashCode()}\n");

                        Student student = new Student(new Person("Alice", "Smith", new DateTime(2002, 5, 10)), Education.Bachelor, 105);
            student.AddExams(new Exam("Math", 4, new DateTime(2023, 12, 20)), new Exam("Physics", 5, new DateTime(2023, 12, 22)));
            student.AddTests(new Test("History", true, new DateTime(2023, 12, 15)), new Test("Literature", false, new DateTime(2023, 12, 18)));
            Console.WriteLine($"Student details:\n{student}\n");

                        Console.WriteLine($"Student's PersonInfo: {student.PersonInfo}\n");

                        Student studentCopy = (Student)student.DeepCopy();
            student.FirstName = "UpdatedFirstName";
            ((Exam)student.Exams[0]).Grade = 3;

            Console.WriteLine($"Original Student:\n{student}\n");
            Console.WriteLine($"Deep Copy of Student:\n{studentCopy}\n");

                        try
            {
                student.GroupNumber = 99;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}\n");
            }

                        Console.WriteLine("All tests and exams:");
            foreach (var item in student.GetAllTestsAndExams())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

                        Console.WriteLine("Exams with grade above 3:");
            foreach (Exam exam in student.GetExamsAboveGrade(3))
            {
                Console.WriteLine(exam);
            }
            Console.WriteLine();

                                    Console.WriteLine("Subjects present in both tests and exams:");
            foreach (string subject in student)
            {
                Console.WriteLine(subject);
            }
            Console.WriteLine();

                        Console.WriteLine("Passed tests and exams:");
            foreach (var item in student.GetPassedTestsAndExams())
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

                        Console.WriteLine("Passed tests with corresponding passed exams:");
            foreach (var test in student.GetPassedTestsWithPassedExams())
            {
                Console.WriteLine(test);
            }
            Console.WriteLine();
        }
    }
}