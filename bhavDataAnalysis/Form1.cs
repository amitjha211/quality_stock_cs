using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bhavDataAnalysis
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        DataTable t = null;

        private void btnOpenCSV_Click(object sender, EventArgs e)
        {
            var oFile = new System.Windows.Forms.OpenFileDialog();

            oFile.Filter = "CSV File|*.csv";

            if (oFile.ShowDialog() == DialogResult.OK)
            {
                lblFilePath.Text = oFile.FileName;
                t = g.readCSV(lblFilePath.Text);
                //dataGridView1.DataSource = getTable();

                t = getTable();

                showColumnsInformation(t);

            }
        }

        private DataTable getTable()
        {
            DataTable tTmp = g.readCSV(lblFilePath.Text);
            foreach (DataColumn col in tTmp.Columns) col.ColumnName = col.ColumnName.Trim();


            t = new DataTable();
            t.TableName = "bhavData";


            t.Columns.Add("SYMBOL", typeof(string));
            t.Columns.Add("SERIES", typeof(string));
            t.Columns.Add("DATE1", typeof(string));
            t.Columns.Add("PREV_CLOSE", typeof(decimal));
            t.Columns.Add("OPEN_PRICE", typeof(decimal));
            t.Columns.Add("HIGH_PRICE", typeof(decimal));
            t.Columns.Add("LOW_PRICE", typeof(decimal));
            t.Columns.Add("LAST_PRICE", typeof(decimal));
            t.Columns.Add("CLOSE_PRICE", typeof(decimal));
            t.Columns.Add("AVG_PRICE", typeof(decimal));
            t.Columns.Add("TTL_TRD_QNTY", typeof(decimal));
            t.Columns.Add("TURNOVER_LACS", typeof(decimal));
            t.Columns.Add("NO_OF_TRADES", typeof(decimal));
            t.Columns.Add("DELIV_QTY", typeof(decimal));
            t.Columns.Add("DELIV_PER", typeof(decimal));

            foreach (DataRow rSource in tTmp.Rows)
            {
                DataRow rDestination = t.NewRow();
                rDestination["SYMBOL"] = rSource["SYMBOL"];
                rDestination["SERIES"] = rSource["SERIES"];
                rDestination["DATE1"] = rSource["DATE1"];

                rDestination["PREV_CLOSE"] = g.parseDecimal(rSource["PREV_CLOSE"], 2);
                rDestination["OPEN_PRICE"] = g.parseDecimal(rSource["OPEN_PRICE"], 2);
                rDestination["HIGH_PRICE"] = g.parseDecimal(rSource["HIGH_PRICE"], 2);
                rDestination["LOW_PRICE"] = g.parseDecimal(rSource["LOW_PRICE"], 2);
                rDestination["LAST_PRICE"] = g.parseDecimal(rSource["LAST_PRICE"], 2);
                rDestination["CLOSE_PRICE"] = g.parseDecimal(rSource["CLOSE_PRICE"], 2);
                rDestination["AVG_PRICE"] = g.parseDecimal(rSource["AVG_PRICE"], 2);
                rDestination["TTL_TRD_QNTY"] = g.parseDecimal(rSource["TTL_TRD_QNTY"], 2);
                rDestination["TURNOVER_LACS"] = g.parseDecimal(rSource["TURNOVER_LACS"], 2);
                rDestination["NO_OF_TRADES"] = g.parseDecimal(rSource["NO_OF_TRADES"], 2);
                rDestination["DELIV_QTY"] = g.parseDecimal(rSource["DELIV_QTY"], 2);
                rDestination["DELIV_PER"] = g.parseDecimal(rSource["DELIV_PER"], 2);

                t.Rows.Add(rDestination);
            }

            return t;
        }

        private void showColumnsInformation(DataTable t)
        {
            foreach (DataColumn col in t.Columns)
            {
                checkedListBox1.Items.Add(col.ColumnName, true);
            }
        }

        private void showData()
        {



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            
        }


        private void color_close_price()
        {
            int iCol = 0;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.DataPropertyName.ToUpper() == "CLOSE_PRICE")
                {
                    iCol = col.Index;
                }
            }
            if(iCol ==0) return ;

            for (int iRow = 0; iRow < dataGridView1.Rows.Count; iRow++)
            {
                DataRowView r = dataGridView1.Rows[iRow].DataBoundItem as DataRowView;
                //if (r == null) return;

                decimal pre_close = (decimal)r["PREV_CLOSE"];

                decimal close_price =(decimal)r["CLOSE_PRICE"];


                if (close_price > pre_close)
                {
                    dataGridView1.Rows[iRow].Cells[iCol].Style.ForeColor = Color.DarkGreen;
                }

                if (close_price < pre_close)
                {
                    dataGridView1.Rows[iRow].Cells[iCol].Style.ForeColor = Color.DarkRed;
                }
                
                
            }
        }



        private void btnShow_Click(object sender, EventArgs e)
        {
            bindingSource1.DataSource = t;
            string qFilter = "";

            qFilter = " DELIV_PER > " + txtDeliveryPer.Text + " and  DELIV_QTY > " + txtVolume.Text;

            if (txtQuickSearch.Text.isEmpty() == false)
            { 
                qFilter += string.Format(" and symbol like '{0}*'",txtQuickSearch.Text);
            }

            bindingSource1.Filter = qFilter;


            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i) == true)
                {
                    string sField = checkedListBox1.GetItemText(checkedListBox1.Items[i]);
                    dataGridView1.Columns.Add(sField, sField);
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].DataPropertyName = sField;

                }
            }

            dataGridView1.DataSource = bindingSource1;

            color_close_price();
        }
    }
}
