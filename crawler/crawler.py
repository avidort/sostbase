import sqlite3


def main():
    print '> Initialising...'
    dbximport('sourceFile.txt')
    populate('sourceFile.txt', 'wildDbName.db')  # todo add argparse instead of hardcoded filenames
    print '\n> Done'


def dbximport(dbxfile):  # todo request dbx permission on first usage
    print '\n> Importing source file from Dropbox ({0})'.format(dbxfile)
    # https://www.dropbox.com/developers/documentation/python#tutorial


def populate(srcfile, destdb):
    print '\n> Populating database ({0} -> {1})'.format(srcfile, destdb)
    try:
        conn = sqlite3.connect(destdb)
        conn.text_factory = str
        c = conn.cursor()
        print '>> Cleaning'
        c.execute('DELETE FROM namedb')
        c.execute("DELETE FROM sqlite_sequence WHERE name = 'namedb'")
        with open(srcfile) as f:
            for line in f:
                print '>> Appending {0}'.format(line.rstrip())
                c.execute('INSERT INTO namedb (name) VALUES (?)', (line.rstrip(),))
        print '>> Saving...'
        conn.commit()
        conn.close()
    except sqlite3.Error as e:
        print "[ERROR] SQLite error,", e.args[0]
    except IOError:
        print "[ERROR] IO error, inaccessible file"

if __name__ == '__main__':
    main()
