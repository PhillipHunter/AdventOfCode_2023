using AOC2023;

namespace AdventOfCode_2023
{
    public partial class AdventForm : Form
    {
        private IAdventPuzzle[] adventPuzzles = AOC.GetAdventPuzzles();

        public AdventForm()
        {
            InitializeComponent();

            lblStatus.Text = string.Empty;

            UpdatePuzzleList();

            cboSelectDay.SelectedIndex = cboSelectDay.Items.Count - 1;
        }
        private void UpdatePuzzleList()
        {
            cboSelectDay.Items.Clear();
            cboSelectDay.DataSource = adventPuzzles;
            cboSelectDay.DisplayMember = "Name";
        }

        private void HandleOutput(PuzzleOutput output)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => HandleOutput(output)));
                return;
            }            

            txtResult.Text = output.Result;
            lblStatus.Text = $"Completed in {output.CompletionTime} ms";

            btnGenerate.Enabled = true;
            cboSelectDay.Enabled = true;
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                IAdventPuzzle selectedPuzzle = (IAdventPuzzle)cboSelectDay.SelectedItem;
                
                lblStatus.Text = "Loading...";
                lblStatus.ForeColor = Color.Black;
                txtResult.Text = String.Empty;
                btnGenerate.Enabled = false;
                cboSelectDay.Enabled = false;

                PuzzleOutput output = await Task.Run(() => selectedPuzzle.GetOutput());
                HandleOutput(output);
            }
            catch (Exception ex)
            {
                txtResult.Text = String.Empty;
                lblStatus.Text = $"{ex.Message}";
                lblStatus.ForeColor = Color.DarkRed;

                btnGenerate.Enabled = true;
                cboSelectDay.Enabled = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void cboSelectDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblStatus.Text = String.Empty;

            if(((IAdventPuzzle)cboSelectDay.SelectedItem).Solution != null)
            {
                lblSolution.Text = "Solution";
                lblSolutionDisplay.Text = ((IAdventPuzzle)cboSelectDay.SelectedItem).Solution;
            }
            else
            {
                lblSolution.Text = string.Empty;
                lblSolutionDisplay.Text = string.Empty;
            }
        }
    }
}