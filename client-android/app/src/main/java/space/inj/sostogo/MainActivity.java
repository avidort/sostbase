package space.inj.sostogo;

import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ProgressBar;
import android.widget.TextView;

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
    TextView curName;
    List<String> names;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        curName = (TextView) findViewById(R.id.curName);
        names = new ArrayList<String>();
        new preloadNames().execute();
    }

    public void onScreenTap(View view) {
        setRandomName();
    }

    public void setRandomName() {
        if (names.size() > 0) curName.setText(names.get(new Random().nextInt(names.size())));
        else curName.setText("N/A");
    }

    public class preloadNames extends AsyncTask<Void, Void, Void> {
        @Override
        protected void onPreExecute() {
            super.onPreExecute();
            ((ProgressBar)findViewById(R.id.progressBar)).setVisibility(View.VISIBLE);
        }

        @Override
        protected Void doInBackground(Void... params) {
            String urlAddress = "http://linkToApi.com";
            URL url;
            HttpURLConnection urlConnection;
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
            return null;
        }

        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            ((ProgressBar)findViewById(R.id.progressBar)).setVisibility(View.GONE);
            setRandomName();
        }
    }
}
