package space.inj.sostogo;

import android.os.StrictMode;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import java.io.IOException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.Scanner;
import java.util.regex.Pattern;

public class MainActivity extends AppCompatActivity {
    List<String> names = new ArrayList<String>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

        preloadNames();
        setRandomName();
    }

    public void onScreenTap(View view) {
        setRandomName();
    }

    public void setRandomName() {
        TextView t = (TextView) findViewById(R.id.curName);
        t.setText(names.get(new Random().nextInt(names.size())));
    }

    public void preloadNames() {
        try {
            URL url = new URL("https://url.to/raw/file.txt"); // TODO: Convert to JSON parsing from new API
            Scanner s = new Scanner(url.openStream());
            s.useDelimiter(Pattern.compile("[\\r\\n;]+"));
            while (s.hasNext()) {
                String line = s.next();
                if (line.length() > 0) names.add(line);
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
