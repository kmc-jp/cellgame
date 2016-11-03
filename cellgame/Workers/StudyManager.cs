using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class StudyManager
    {
        #region Variable
        // 現在行われている研究
        public static Study studying;
        // その研究が完了しているかどうか
        static bool[] StudyState = {
            false, false, false, false, false, false, false, false, false, false, false
        };

        // 進行具合
        static int _studyPower;
        public static int StudyPower
        {
            get { return _studyPower; }
            set { _studyPower = Math.Max(0, Math.Min(value, DataBase.maxStudyPower[(int)studying])); }
        }
        // 研究名
        public static string StudyName
        {
            get { return DataBase.StudyName[(int)studying]; }
        }
        // 最大研究値
        public static int MaxStudyPower
        {
            get { return DataBase.maxStudyPower[(int)studying]; }
        }
        // 残りターン
        public static int LeftTurn
        {
            get { return (DataBase.maxStudyPower[(int)studying] - StudyPower + PlayScene.studyPower - 1) / PlayScene.studyPower; }
        }
        #endregion

        #region Method
        // コンストラクタ
        StudyManager() {
            studying = Study.Kaku;
            StudyPower = 0;
        }

        // その研究が終わっているかどうか
        public static bool IsDone(Study st)
        {
            return StudyState[(int)st];
        }
        // 研究を終える
        static void Do()
        {
            StudyState[(int)studying] = true;
            
            switch (studying)
            {
                case Study.Kaku:
                    ProductScene.productable = 7;
                    break;
                case Study.Saito:
                    ProductScene.productable = 8;
                    break;
                case Study.Inter:
                    for (int i = 0; i < 11; i++)
                        DataBase.maxStudyPower[i] = DataBase.maxStudyPower[i] * 4 / 5;
                    break;
                case Study.Kemo:
                    for (int i = 0; i < 8; i++)
                        DataBase.sumProductPower[i] -= DataBase.maxProductPower[i];
                    break;
                case Study.Cross:
                    DataBase.sumProductPower[(int)UnitType.KillerT - 1] -= DataBase.maxProductPower[(int)UnitType.KillerT - 1] * 2;
                    DataBase.sumProductPower[(int)UnitType.NK - 1] -= DataBase.maxProductPower[(int)UnitType.NK - 1] * 2;
                    break;
                case Study.Kou:
                    DataBase.MyUnitStrength[(int)UnitType.KillerT - 1] = DataBase.MyUnitStrength[(int)UnitType.KillerT - 1] * 4 / 3;
                    DataBase.MyUnitStrength[(int)UnitType.NK - 1] = DataBase.MyUnitStrength[(int)UnitType.NK - 1] * 4 / 3;
                    break;
                case Study.Shinwa:
                    DataBase.MyUnitStrength[(int)UnitType.B - 1] *= 2;
                    break;
                case Study.Masuto:
                    DataBase.EnemyUnitStrength[(int)UnitType.Kiseichu + 5] = DataBase.EnemyUnitStrength[(int)UnitType.Kiseichu + 5] * 3 / 4;
                    break;
                default:
                    break;
            }
        }
        public static bool StartStudying(Study st)
        {
            if (studying == st) return false;

            StudyPower = 0;
            studying = st;

            return true;
        }
        public static void UpdateTurn()
        {
            StudyPower += PlayScene.studyPower;

            if(StudyPower == MaxStudyPower && !StudyState[(int)studying])
            {
                Do();
            }
        }
        #endregion
    }
}
