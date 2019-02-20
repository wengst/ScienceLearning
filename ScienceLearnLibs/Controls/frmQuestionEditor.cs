using LearnLibs.Enums;
using LearnLibs.Models;
using System;

namespace LearnLibs.Controls
{
    public partial class frmQuestionEditor : LearnLibs.Controls.FormDialog
    {
        Question ques = null;
        public override object Object
        {
            get
            {
                if (ques != null)
                {
                    ques.AnswerMode = ModelDbSet.GetSelectedEnum<AnswerMode>(lcbAnswerMode.CMB, AnswerMode.选择);
                    ques.Difficult = (float)ltnDifficult.Value;
                    ques.Score = (float)ltnScore.Value;
                    ques.OptionChars = ltbOptionChars.Text;
                    ques.KeyChars = ltbKeyChars.Text;
                    return ques;
                }
                else
                {
                    throw new ArgumentNullException("习题小问不能为NULL");
                }
            }
            set
            {
                if (value != null && value.GetType() == typeof(Question))
                {
                    ques = (Question)value;
                    if (ques.SubjectId != Guid.Empty)
                    {
                        ltbOptionChars.Text = ques.OptionChars;
                        ltbKeyChars.Text = ques.KeyChars;
                        ModelDbSet.SetSelectedEnum<AnswerMode>(lcbAnswerMode.CMB, ques.AnswerMode);
                        ltnDifficult.Value = (decimal)ques.Difficult;
                        ltnScore.Value = (decimal)ques.Score;
                    }
                    else {
                        throw new ArgumentException("习题小问的习题ID不能为Guid.Empty");
                    }
                }
                else {
                    throw new Exception("实例为NULL或者实例类型不是Question");
                }
            }
        }
        public frmQuestionEditor()
        {
            InitializeComponent();
            ModelDbSet.BindComboBoxByEmuns<AnswerMode>(lcbAnswerMode.CMB);
        }
    }
}
