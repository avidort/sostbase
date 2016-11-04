package space.inj.sostogo;

import android.content.DialogInterface;
import android.net.Uri;
import android.os.AsyncTask;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.InputType;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class MainActivity extends AppCompatActivity {
    TextView curName, totalTV, remainsTV;
    List<String> names;
    private String m_Text = "";
    public String urlAddress = "http://sos.inj.space";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        curName = (TextView) findViewById(R.id.curName);
        totalTV = (TextView) findViewById(R.id.totalTV);
        remainsTV = (TextView) findViewById(R.id.remainsTV);
        names = new ArrayList<>();
        new preloadNames().execute();
    }

    public void onScreenTap(View view) {
        if(names.size() > 0)
            remainsTV.setText("Remaining names: " + names.size());
        setRandomName();
    }

    public void setRandomName() {
        if (names.size() > 0) {
            int random = new Random().nextInt((names.size()));
            curName.setText(names.get(random));
            names.remove(random);
        }
        else curName.setText("N/A");
    }

    public void addName(View view) {
        final Uri.Builder urlBuild = new Uri.Builder();
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setTitle("Add name...");

        final EditText inputET = new EditText(this);
        inputET.setInputType(InputType.TYPE_CLASS_TEXT);
        builder.setView(inputET);

        builder.setPositiveButton("Add", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                urlBuild.appendPath(urlAddress); // TODO: Add a url with a key
                m_Text = inputET.getText().toString();
                Toast.makeText(getApplicationContext(), "Added" + inputET, Toast.LENGTH_SHORT).show();
            }
        });
        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                Toast.makeText(MainActivity.this, "Canceled", Toast.LENGTH_SHORT).show();
            }
        });

        builder.show();
    }

    public class preloadNames extends AsyncTask<Void, Void, Void> {
        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            ((ProgressBar)findViewById(R.id.progressBar)).setVisibility(View.VISIBLE);
        }

        @Override
        protected Void doInBackground(Void... params) {
            URL url;
            HttpURLConnection urlConnection = null;
            BufferedReader reader;
            String sosForecast;
            try {
                url = new URL(urlAddress);
                urlConnection = (HttpURLConnection) url.openConnection();
                urlConnection.setRequestMethod("GET");
                urlConnection.connect();
                reader = new BufferedReader(new InputStreamReader(urlConnection.getInputStream()));
                sosForecast = reader.readLine();
                try {
                    JSONArray jsonArray = new JSONArray(sosForecast);
                    for(int i = 0; i < jsonArray.length(); i++) {
                        JSONObject jsonObject = jsonArray.getJSONObject(i);
                        names.add(jsonObject.getString("name"));
                    }
                } catch (JSONException e) {
                    Log.e("MainActivity", "Error", e);
                }

            } catch (IOException e) {
                e.printStackTrace();
            }

            finally {
                if(urlConnection != null) urlConnection.disconnect();
            }

            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            ((ProgressBar) findViewById(R.id.progressBar)).setVisibility(View.GONE);
            totalTV.setText("Total names: " + String.valueOf(names.size()));
            setRandomName();
        }
    }
}
