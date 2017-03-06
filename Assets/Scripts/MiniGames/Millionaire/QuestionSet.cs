using System.Collections.Generic;

public class MillionaireGameData {
    private List<QuestionSet> _questionSets = new List<QuestionSet>();

    public List<QuestionSet> QuestionSets {
        get { return _questionSets; }
    }
}

public class QuestionSet {

    public string Title { get; private set; }

    public string Category { get; private set; }

    public string Description { get; private set; }

    public string PreviewImagePath { get; private set; }

    public Level[] Levels { get; private set; }

    public QuestionSet(string title, string image, string description, string category, Level[] levels) {
        Title = title;
        PreviewImagePath = image;
        Description = description;
        Category = category;
        Levels = levels;
    }
}

public class Level {

    public Question[] questions { get; set; }

    public Level(int number_of_questions) {
        questions = new Question[number_of_questions];
    }
}

public class Question {

    public string content { get; set; }

    public string imagePath { get; set; }

    public double imgWidth { get; set; }

    public double imgHeight { get; set; }

    public Answer[] answers { get; set; }

    public Question() {
        answers = new Answer[4];
    }
}

public class Answer {

    public string content { get; set; }

    public bool correct { get; set; }

    public Answer() {
    }
}