public interface Observer
{
    void UpdateTime(float gameTime); // 진행시간
    void UpdateCombo(int comboNum, int maxComboNum); // 콤보
}
