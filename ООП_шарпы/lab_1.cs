namespace lab_1
{
    public enum Education
    {
        Specialist,
        Bachelor,
        SecondEducation
    }

    public class Person
    {
        private string firstName = string.Empty;
        private string lastName;
        private DateTime birthDate;

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
            return $"Name: {firstName}, Last name: {lastName}, BirthDate {birthDate.ToShortDateString()}";
        }

        public virtual string ToShortString()
        {
            return $"{firstName} {lastName}";
        }
    }

    public class Exam
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
    }

    public class Student
    {
        private Person person;
        private Education education;
        private int groupNumber;
        private List<Exam> exams;

        public Student(Person person, Education education, int groupNumber)
        {
            this.person = person;
            this.education = education;
            this.groupNumber = groupNumber;
            this.exams = new List<Exam>();
        }

        public Student()
        {
            this.person = new Person();
            this.education = Education.Bachelor;
            this.groupNumber = 0;
            this.exams = new List<Exam>();
        }

        public Person PersonInfo
        {
            get { return person; }
            set { person = value; }
        }

        public Education EducationForm
        {
            get { return education; }
            set { education = value; }
        }

        public int GroupNumber
        {
            get { return groupNumber; }
            set { groupNumber = value; }
        }

        public List<Exam> Exams
        {
            get { return exams; }
            set { exams = value; }
        }

        public double AverageGrade
        {
            get
            {
                if (exams.Count == 0) return 0.0;

                double sum = 0.0;
                foreach (var exam in exams)
                {
                    sum += exam.Grade;
                }

                return sum / exams.Count;
            }
        }

        public bool this[Education educationForm]
        {
            get { return education == educationForm; }
        }

        public void AddExams(params Exam[] exam)
        {
            exams.AddRange(exam);
        }

        public override string ToString()
        {
            string examsInfo = "";
            foreach (var exam in exams)
            {
                examsInfo += exam.ToString() + "\n";
            }

            return $"Student: {person}\nEducation: {education}, Group: {groupNumber}, Average Grade: {AverageGrade}\nExams:\n{examsInfo}";
        }

        public virtual string ToShortString()
        {
            return $"Student: {person}\nEducation: {education}\nGroup: {groupNumber}, Average Grade: {AverageGrade}";
        }
    }

    public class Program
    {
        public static void Main()
        {
            var student = new Student();
            Console.WriteLine($"Student (ToShortString): {student.ToShortString()}\n");

            Console.WriteLine("Education form check:");
            Console.WriteLine($"Is Specialist: {student[Education.Specialist]}");
            Console.WriteLine($"Is Bachelor: {student[Education.Bachelor]}");
            Console.WriteLine($"Is SecondEducation: {student[Education.SecondEducation]}");
            Console.WriteLine();

            student.PersonInfo = new Person("Doe", "Bidon", new DateTime(2000, 1, 1));
            student.EducationForm = Education.Bachelor;
            student.GroupNumber = 101;
            Console.WriteLine($"Student (ToString) after setting properties: {student.ToShortString()}\n");

            student.AddExams(
                new Exam("Mathematics", 90, new DateTime(2024, 12, 1)),
                new Exam("Physics", 85, new DateTime(2024, 12, 15)),
                new Exam("Chemistry", 92, new DateTime(2024, 12, 20))
            );
            Console.WriteLine($"Student (ToString) after adding exams: {student.ToString()}\n");

            const int size = 1000000;

            Exam[] oneDimensionalArray = new Exam[size];
            for (int i = 0; i < size; i++)
            {
                oneDimensionalArray[i] = new Exam("Subject", 100, DateTime.Now);
            }

            int matrixSize = (int)Math.Sqrt(size);
            Exam[,] rectangularArray = new Exam[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    rectangularArray[i, j] = new Exam("Subject", 100, DateTime.Now);
                }
            }

            Exam[][] jaggedArray = new Exam[matrixSize][];
            for (int i = 0; i < matrixSize; i++)
            {
                jaggedArray[i] = new Exam[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    jaggedArray[i][j] = new Exam("Subject", 100, DateTime.Now);
                }
            }

            var startTime = DateTime.Now;
            foreach (var exam in oneDimensionalArray)
            {
                var grade = exam.Grade;
            }
            var elapsedOneDimensional = DateTime.Now - startTime;

            startTime = DateTime.Now;
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    var grade = rectangularArray[i, j].Grade;
                }
            }
            var elapsedRectangular = DateTime.Now - startTime;

            startTime = DateTime.Now;
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    var grade = jaggedArray[i][j].Grade;
                }
            }
            var elapsedJagged = DateTime.Now - startTime;

            Console.WriteLine("Array Access Time:");
            Console.WriteLine($"One-dimensional array: {elapsedOneDimensional.TotalMilliseconds} ms");
            Console.WriteLine($"Rectangular two-dimensional array: {elapsedRectangular.TotalMilliseconds} ms");
            Console.WriteLine($"Jagged two-dimensional array: {elapsedJagged.TotalMilliseconds} ms");
        }
    }
}