from timetable_extractor import (
    extract_timetable,
    build_room_timetable,
    print_room_timetable
)

from calendar_extractor import (
    extract_calendar_days,
    print_calendar_days
)

from json_writer import save_json


TIMETABLE_PATH = "input/timetable.pdf"
CALENDAR_PATH = "input/academic_calendar.pdf"

TIMETABLE_OUTPUT = "output/timetable.json"
ROOM_TIMETABLE_OUTPUT = "output/room_timetable.json"
CALENDAR_OUTPUT = "output/calendar_days.json"


def main():
    timetable = extract_timetable(TIMETABLE_PATH)
    room_timetable = build_room_timetable(timetable)

    calendar_days = extract_calendar_days(CALENDAR_PATH)

    print_room_timetable(room_timetable)
    print_calendar_days(calendar_days)

    save_json(timetable, TIMETABLE_OUTPUT)
    save_json(room_timetable, ROOM_TIMETABLE_OUTPUT)
    save_json(calendar_days, CALENDAR_OUTPUT)

    print("\nJSON出力が完了しました。")


if __name__ == "__main__":
    main()