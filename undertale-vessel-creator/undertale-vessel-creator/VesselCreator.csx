// Vessel Creator for Undertale - UndertaleModTool script
// Adds a Deltarune-style "create your vessel" screen to the start room.
// Run via UndertaleModTool: Scripts > Run other script... > VesselCreator.csx
// Written for the GMS 1.4 era Steam build of Undertale (v1.08), no room layers.

using System;
using System.Linq;
using UndertaleModLib.Models;

EnsureDataLoaded();

const string TARGET_ROOM = "room_start";   // room where the creator appears. Change if needed.
const string OBJ_NAME = "obj_vessel_creator";

if (Data.GameObjects.ByName(OBJ_NAME) != null)
{
    ScriptError("Vessel Creator is already installed in this data.win.");
    return;
}

var room = Data.Rooms.ByName(TARGET_ROOM);
if (room == null)
{
    ScriptError("Room '" + TARGET_ROOM + "' not found. Open data.win in UndertaleModTool, " +
                "look under Rooms, and set TARGET_ROOM at the top of this script.");
    return;
}

// ---------- GML ----------
string createCode = @"
global.vessel_active = true;
global.vessel_name = ""FRISK"";
global.vessel_color = c_red;
global.vessel_shape = 0;
ini_open(""vessel.ini"");
if (ini_read_real(""vessel"", ""saved"", 0) == 1)
{
    global.vessel_name = ini_read_string(""vessel"", ""name"", ""FRISK"");
    global.vessel_color = ini_read_real(""vessel"", ""color"", c_red);
    global.vessel_shape = ini_read_real(""vessel"", ""shape"", 0);
    global.vessel_active = false;   // already created, skip the screen
}
ini_close();
colors[0] = c_red;  colors[1] = c_aqua; colors[2] = c_green; colors[3] = c_orange;
colors[4] = c_yellow; colors[5] = c_purple; colors[6] = c_blue; colors[7] = c_white;
cidx = 0;
row = 0;          // 0 = name, 1 = color, 2 = shape, 3 = confirm
depth = -100000;
persistent = true;
if (!global.vessel_active) instance_destroy();
";

string stepCode = @"
if (!global.vessel_active) exit;
if (keyboard_check_pressed(vk_down))  row = (row + 1) mod 4;
if (keyboard_check_pressed(vk_up))    row = (row + 3) mod 4;
if (row == 1)
{
    if (keyboard_check_pressed(vk_right)) cidx = (cidx + 1) mod 8;
    if (keyboard_check_pressed(vk_left))  cidx = (cidx + 7) mod 8;
    global.vessel_color = colors[cidx];
}
if (row == 2)
{
    if (keyboard_check_pressed(vk_right)) global.vessel_shape = (global.vessel_shape + 1) mod 3;
    if (keyboard_check_pressed(vk_left))  global.vessel_shape = (global.vessel_shape + 2) mod 3;
}
if (row == 0)
{
    var k = keyboard_lastchar;
    if (keyboard_check_pressed(vk_backspace))
        global.vessel_name = string_copy(global.vessel_name, 1, string_length(global.vessel_name) - 1);
    else if (keyboard_check_pressed(vk_anykey) && string_length(global.vessel_name) < 6
             && string_length(k) == 1 && string_letters(k) != """")
        global.vessel_name += string_upper(k);
}
if (row == 3 && (keyboard_check_pressed(vk_space) || keyboard_check_pressed(vk_tab)))
{
    ini_open(""vessel.ini"");
    ini_write_real(""vessel"", ""saved"", 1);
    ini_write_string(""vessel"", ""name"", global.vessel_name);
    ini_write_real(""vessel"", ""color"", global.vessel_color);
    ini_write_real(""vessel"", ""shape"", global.vessel_shape);
    ini_close();
    global.vessel_active = false;
    keyboard_clear(vk_anykey);
    instance_destroy();
}
keyboard_lastchar = """";
";

string drawCode = @"
if (!global.vessel_active) exit;
draw_set_color(c_black);
draw_rectangle(0, 0, 640, 480, false);
draw_set_font(fnt_maintext);
draw_set_color(c_white);
draw_text(180, 30, ""CREATE YOUR VESSEL"");
var labels;
labels[0] = ""NAME: "" + global.vessel_name;
labels[1] = ""SOUL COLOR"";
labels[2] = ""SHAPE: "" + string(global.vessel_shape + 1);
labels[3] = ""CONFIRM  (SPACE)"";
for (var i = 0; i < 4; i += 1)
{
    draw_set_color(i == row ? c_yellow : c_white);
    draw_text(200, 110 + i * 40, labels[i]);
}
// soul preview
draw_set_color(global.vessel_color);
var cx = 320, cy = 340;
if (global.vessel_shape == 0) draw_circle(cx, cy, 18, false);
if (global.vessel_shape == 1) draw_rectangle(cx - 16, cy - 16, cx + 16, cy + 16, false);
if (global.vessel_shape == 2) draw_triangle(cx, cy - 20, cx - 20, cy + 16, cx + 20, cy + 16, false);
draw_set_color(c_white);
draw_text(150, 420, ""UP/DOWN: select    LEFT/RIGHT: change"");
";

// ---------- create object ----------
var obj = new UndertaleGameObject();
obj.Name = Data.Strings.MakeString(OBJ_NAME);
Data.GameObjects.Add(obj);

obj.EventHandlerFor(EventType.Create, Data.Strings, Data.Code, Data.CodeLocals).ReplaceGML(createCode, Data);
obj.EventHandlerFor(EventType.Step, EventSubtypeStep.Step, Data.Strings, Data.Code, Data.CodeLocals).ReplaceGML(stepCode, Data);
obj.EventHandlerFor(EventType.Draw, EventSubtypeDraw.Draw, Data.Strings, Data.Code, Data.CodeLocals).ReplaceGML(drawCode, Data);

// ---------- place it in the room ----------
var inst = new UndertaleRoom.GameObject();
inst.X = 0;
inst.Y = 0;
inst.ObjectDefinition = obj;
inst.InstanceID = Data.GeneralInfo.LastObj++;
room.GameObjects.Add(inst);

// ---------- tint the battle soul ----------
var heart = Data.GameObjects.ByName("obj_heart");
if (heart != null)
{
    try
    {
        var h = heart.EventHandlerFor(EventType.Create, Data.Strings, Data.Code, Data.CodeLocals);
        h.AppendGML("if (variable_global_exists(\"vessel_color\")) image_blend = global.vessel_color;", Data);
    }
    catch (Exception e) { ScriptMessage("Created the screen, but could not tint obj_heart: " + e.Message); }
}

ScriptMessage("Vessel Creator installed into " + TARGET_ROOM + ". Save data.win (File > Save) and run the game.");
