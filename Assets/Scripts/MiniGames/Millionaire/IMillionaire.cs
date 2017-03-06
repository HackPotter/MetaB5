public interface IMillionaire {

    int[] eliminate();

    string[] get_answers();

    int get_correct_answer();

    int get_final_score();

    string get_game_title();

    string get_image_path();

    int get_level();

    int get_max_level();

    string get_question();

    int get_score();

    bool is_correct_answer(int answer_id);

    void next_level();

    void reset();

    void switch_question();

    void walk_away();
}
