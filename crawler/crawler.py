import sqlite3


def main():
    # todo read remote .txt file from dropbox using their API @ https://www.dropbox.com/developers-v1/core/start/python
    print '> Initialising...'
    convert('sourceFile.txt', '../server/wildDbName.db')


def convert(srcfile, destdb):
    try:
        conn = sqlite3.connect(destdb)
        conn.text_factory = str
        c = conn.cursor()
        c.execute('DELETE FROM namedb')
        c.execute("DELETE FROM sqlite_sequence WHERE name = 'namedb'")
        with open(srcfile) as f:
            for line in f:
                print '>> Appending {0}'.format(line.rstrip())
                c.execute('INSERT INTO namedb (name) VALUES (?)', (line,))
        conn.commit()
        conn.close()
    except sqlite3.Error as e:
        print "[ERROR] SQLite error,", e.args[0]
    except IOError:
        print "[ERROR] IO error, inaccessible file"

if __name__ == '__main__':
    main()
