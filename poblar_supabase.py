import psycopg2

try:
    conn = psycopg2.connect(
        host="aws-0-us-east-2.pooler.supabase.com",
        port=6543,
        dbname="postgres",
        user="postgres.redmodonvzimywtgonjt",
        password="Perrohappy",
        sslmode="require"
    )
    print("¡Conexión exitosa!")
    cur = conn.cursor()

    # Limpia las tablas antes de poblar
    cur.execute('DELETE FROM lessons')
    cur.execute('DELETE FROM modules')
    cur.execute('DELETE FROM courses')
    cur.execute('DELETE FROM instructors')
    conn.commit()
    print("¡Tablas limpiadas!")

    # 1. Insertar Instructores
    for i in range(1, 101):
        nombre = f"Instructor {i}"
        cur.execute('INSERT INTO instructors (name) VALUES (%s)', (nombre,))

    # 2. Insertar Cursos (asignando instructores del 1 al 100)
    for i in range(1, 101):
        title = f"Course {i}"
        is_published = False
        instructor_id = i
        cur.execute('INSERT INTO courses (title, is_published, instructor_id) VALUES (%s, %s, %s)', (title, is_published, instructor_id))

    # 3. Insertar Módulos (asignando cursos del 1 al 100)
    for i in range(1, 101):
        title = f"Module {i}"
        course_id = i
        cur.execute('INSERT INTO modules (title, course_id) VALUES (%s, %s)', (title, course_id))

    # 4. Insertar Lecciones (asignando módulos del 1 al 100)
    for i in range(1, 101):
        title = f"Lesson {i}"
        content = f"Content of lesson {i}"
        module_id = i
        cur.execute('INSERT INTO lessons (title, content, module_id) VALUES (%s, %s, %s)', (title, content, module_id))

    conn.commit()
    print("¡Datos insertados correctamente!")
    cur.close()
    conn.close()
except Exception as e:
    print("Error:", e)