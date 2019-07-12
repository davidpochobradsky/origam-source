﻿using Origam.Schema.EntityModel;
using System.Collections;
using System.Windows.Forms;

namespace Origam.UI.WizardForm
{
    public class ScreenWizardForm : AbstractWizardForm
    {
        public IDataEntity Entity { get; set; }
        public bool IsRoleVisible { get; set; }
        public bool textColumnsOnly { get; set; }
        private CheckedListBox _lstFields;

        public void SetUpForm(CheckedListBox lstField)
        {
            _lstFields = lstField;
            _lstFields.Items.Clear();

            if (this.Entity == null) return;

            foreach (IDataEntityColumn column in this.Entity.EntityColumns)
            {
                if (string.IsNullOrEmpty(column.ToString())) continue;
                if (!this.textColumnsOnly
                    || (column.DataType == Origam.Schema.OrigamDataType.String
                    || column.DataType == Origam.Schema.OrigamDataType.Memo))
                {
                    _lstFields.Items.Add(column);
                }
            }
        }
        public Hashtable SelectedFieldNames
        {
            get
            {
                Hashtable result = new Hashtable();

                foreach (IDataEntityColumn column in _lstFields.CheckedItems)
                {
                    result.Add(column.Name, null);
                }

                return result;
            }
        }
    }
}
