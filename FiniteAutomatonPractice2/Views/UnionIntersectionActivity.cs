using Android.App;
using Android.OS;

namespace FiniteAutomatonPractice2.Views
{
    [Activity(Label = "Probar Automata Finito")]
    public class UnionIntersectionActivity : Activity
    {
        string serializedAutomaton;
        string serializedAutomaton1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.union_intersection_activity);

            serializedAutomaton = Intent.GetStringExtra("serializedAutomaton");
            serializedAutomaton1 = Intent.GetStringExtra("serializedAutomaton1");
        }
    }
}