' ===================== USER DEFINED SETTINGS =======================================================

        dim playersGUItitle_InputName as string = "PlayersGUI"

' ===================== USER DEFINED SETTINGS =======================================================
 
' ===================== DO NOT TOUCH SETTINGS =======================================================
    ' ==============================================================
        dim intervalDefault as integer = 500

        dim interval as integer = intervalDefault

        dim isError as boolean = false 'error state of check
        
        dim previewInputError as boolean = false
        
        dim PRVColor as string = "#00BF00"
        dim PGMColor as string = "#BF0000"
        dim FREEColor as string = "#FFFFFF"



        dim yellowAlarmTime as integer = 10000 ' 10 sec
        dim yellowAlarmcolor as string = "#FFFF00"
        dim redAlarmTime as integer = 5000
        dim redAlarmColor as string = "#FF0000"
        dim noAlarmColor as string = "#FFFFFF"

        dim defaultListFontSize as integer = 20
        dim maxDefaultListItems as integer = 12 

        dim playing as string = "Now Playing..."
        dim paused as string = "Paused"
        dim completed as string = "Completed"

        dim PRV as string = "PRV"
        dim PGM as string = "PGM"

        dim listType as string = "VideoList"
        dim selectedClipSymbol as string = "==>"

        dim config  as new system.xml.xmldocument

        config.loadxml(API.XML)

        dim playersGUItitle_InpuntNode as XMLNode = config.SelectSingleNode("/vmix/inputs/input[@shortTitle=""" & playersGUItitle_InputName & """]")
        dim playersGUItitle_Input   as object

        if playersGUItitle_InpuntNode isnot Nothing  Then 
            playersGUItitle_Input  = Input.Find(playersGUItitle_InputName)
        else 
            isError = true
            console.wl("ERROR (line " & me.CurrentLine.tostring() & "): " & "Seems, that no XAML input with name: " & playersGUItitle_InputName )
        end if
    ' ==============================================================

    if not isError

    ' ==============================================================
    while not isError
        ' ====== base functions
            dim players()  as string = { playersGUItitle_Input.Text("txt_playerInput_1") , playersGUItitle_Input.Text("txt_playerInput_2")}
            
            config.loadxml(API.XML)
            dim progressBarWidthString as string = playersGUItitle_Input.Text("txt_widthOfProgressBar")
            dim progressBarWidth as double = Convert.toDouble(progressBarWidthString)
            dim heigthOfMetersString as string = playersGUItitle_Input.Text("txt_heigthOfMeters")
            dim heigthOfMeters as double = Convert.toDouble(heigthOfMetersString)

        
        ' ==============================================
        ' ============ PRV/PGM =========================
            'The KEYS of PROGRAM and PREVIEW
            dim PROGInputName   as string = ""              ' The ShortTitle (AS String) of the INPUT NOW in PROGRAM
            dim PREVInputName   as string = ""              ' The ShortTitle (AS String) of the INPUT NOW in PREVIEW
            dim PROGInputGUID    as string = ""              ' The KEY (AS String) of the INPUT NOW in PROGRAM
            dim PREVInputGUID    as string = ""              ' The KEY (AS String) of the INPUT NOW in PROGRAM
            dim PROGInputType   as string = ""
            dim PREVInputType   as string = ""


            'INPUT NUMBERS of PROGRAM and PREVIEW
            dim PROGInputNumber as string = ""      ' TPROGInputhe NUMBER (AS String) of the INPUT NOW in PROGRAM
            dim PREVInputNumber as string = ""      ' The NUMBER (AS String) of the INPUT NOW in PREVIEW

            dim PROGInputNodeList   as XMLNodeList          ' NodeList of PROGRAM layers
            dim PROGInputNode       as XMLNode              ' The XMLNode of PROGRAM INPUT
            dim PREVInputNodeList   as XMLNodeList          ' NodeList of PREVIEW layers
            dim PREVInputNode       as XMLNode              ' The XMLNode of PREVIEW INPUT

            'Get the XMLNode for the Input in PROGRAM:
            PROGInputNumber = config.SelectSingleNode("/vmix/active").InnerText
            PROGInputNode   = config.SelectSingleNode("/vmix/inputs/input[@number=" & PROGInputNumber & "]")
            PROGInputType   = PROGInputNode.Attributes.GetNamedItem("type").Value
            PROGInputName   = PROGInputNode.Attributes.GetNamedItem("shortTitle").Value
            PROGInputGUID    = PROGInputNode.Attributes.GetNamedItem("key").Value

            'Get the XMLNode for the Input in PREVIEW:
            PREVInputNumber = config.SelectSingleNode("/vmix/preview").InnerText
            PREVInputNode   = config.SelectSingleNode("/vmix/inputs/input[@number=" & PREVInputNumber & "]")
            PREVInputType   = PREVInputNode.Attributes.GetNamedItem("type").Value
            PREVInputName   = PREVInputNode.Attributes.GetNamedItem("shortTitle").Value
            PREVInputGUID    = PREVInputNode.Attributes.GetNamedItem("key").Value

            'The XMLNodeList of LAYERS in PROGRAM:
            PROGInputNodeList = config.SelectSingleNode("/vmix/inputs/input[@key=""" & PROGInputGUID & """]").SelectNodes("overlay") 

            'The XMLNodeList of LAYERS in PREVIEW:
            PREVInputNodeList = config.SelectSingleNode("/vmix/inputs/input[@key=""" & PREVInputGUID & """]").SelectNodes("overlay") 
        ' ==============================================
        
        
        
        
        ' ======= individual ====================
            for i as Integer = 0 to (players.length - 1)
                ' used variables
                    dim fields as New System.Collections.Generic.Dictionary(Of String, String)

                    fields.add("txt_playerInputError_","")
                    fields.add("txt_playerError_","")
                    fields.add("txt_colorStroke_", FREEColor)
                    fields.add("txt_status_", paused)
                    fields.add("txt_playerFileName_", "")
                    fields.add("txt_listNumbers_", "")
                    fields.add("txt_listNames_", "")
                    fields.add("txt_listFontSize_", defaultListFontSize.toString())


                    fields.add("txt_duration_", "")
                    fields.add("txt_curr_", "")
                    fields.add("txt_progress_", progressBarWidthString)
                    fields.add("txt_in_", "0")
                    fields.add("txt_out_", progressBarWidthString)
                    fields.add("txt_inTime_", "")
                    fields.add("txt_outTime_", "")

                    fields.add("txt_meterF1_", "0")
                    fields.add("txt_meterF2_", "0")



                    dim keyList as new System.Collections.Generic.List(Of String)(fields.Keys)  



                    dim playersPreview_InputName as string = playersGUItitle_Input.Text("txt_previewInput")

                ' ============== playersPreview_InputName ================
                    dim playersPreview_InpuntNode as XMLNode
                    
                    if  playersPreview_InputName <> "" then
                        playersPreview_InpuntNode = config.SelectSingleNode("/vmix/inputs/input[@shortTitle=""" & playersPreview_InputName & """]")
                        if playersPreview_InpuntNode isnot Nothing then
                            previewInputError = false
                        else 
                            previewInputError = true
                            fields("txt_playerError_") = "THERE IS NO INPUT FOR PREVIEW WITH NAME: " & playersPreview_InputName
                        end if 
                    else 
                        previewInputError = true
                        fields("txt_playerError_") = "PLAYERS PREVIEW INPUT NAME IS EMPTY"
                    end if 
                ' ======================================================
                ' =============== interval ====================

                    dim intervalString as string = playersGUItitle_Input.Text("txt_interval")
                    
                    if Int16.TryParse(intervalString, interval) then
                        if (interval < 100) then
                            interval = intervalDefault
                            fields("txt_playerError_") += Environment.NewLine & "INTERVAL IS TOO SMALL. Interval is set to defailt: " & intervalDefault.tostring() & "msec."
                        end if 
                    else
                        interval = intervalDefault
                        fields("txt_playerError_") += Environment.NewLine & "INTERVAL PARSE FILED. Interval is set to defailt: " & intervalDefault.tostring() & "msec."
                    end if 
                    dim index as integer = i + 1
                    
                    dim playerInputError as boolean = false

                    dim playerInputLayer as integer
                    dim playerInputLayerString as string = playersGUItitle_Input.Text("txt_layerNumber_" & index.tostring())
                    
                    dim player_InpuntNode as XMLNode = config.SelectSingleNode("/vmix/inputs/input[@shortTitle=""" & players(i) & """]")
                    dim player_GUID as string = ""
                    dim player_Type as string = ""

                    dim tally as string = "" ' "PGM" or "PRV"



                ' ============== targetLayer Check ===============
                    if Int16.TryParse(playerInputLayerString, playerInputLayer) then
                        if playerInputLayer > 10 or playerInputLayer <= 0 then 
                            playerInputLayer = index
                            fields("txt_playerError_") += Environment.NewLine & "Target Layer for " & players(i) & " should be [1-10]. Target Layer is set to: " & playerInputLayer.tostring()
                        end if
                    else 
                        playerInputLayer = index
                        fields("txt_playerError_") += Environment.NewLine & "Target Layer for " & players(i) & " is INCORRECT. Target Layer is set to: " & playerInputLayer.tostring()
                    end if
                ' ==========================================         
                ' ==================== selected name check =============================
                    if player_InpuntNode isnot Nothing then   
                        playerInputError = false
                        player_GUID = player_InpuntNode.Attributes.GetNamedItem("key").Value

                        if players(i) = playersPreview_InputName then
                            fields("txt_playerError_") += Environment.NewLine & "You choose Players Preview Input as player! Select another input!"
                            players(i) = ""
                        end if

                        if players(i) = playersGUItitle_InputName then
                            fields("txt_playerError_") += Environment.NewLine & "You choose PlayersGUI Input as player! Select another input!"
                            players(i) = ""
                        end if
                    else
                        fields("txt_playerInputError_") = "THERE IS NO INPUT WITH THIS NAME"
                        playerInputError = true
                    end if
                ' =======================================================================
                ' =============== Tally Stuff =============    
                    ' check layers 
                    if players(i) <> ""
                        for each layer as XMLNode in PROGInputNodeList
                            if layer.Attributes.GetNamedItem("key").Value = player_GUID then
                                tally = PGM
                            end if
                        next 
                        if tally <> PGM
                            for each layer as XMLnode in PREVInputNodeList
                                if layer.Attributes.GetNamedItem("key").Value = player_GUID then
                                    tally = PRV
                                end if
                            next 
                            if tally <> PGM then
                                if player_GUID = PROGInputGUID then 
                                    tally = PGM
                                else 
                                    if player_GUID = PREVInputGUID
                                        tally = PRV 
                                    end if
                                end if
                            end if
                        end if
                    end if 

                    select tally
                        case PRV
                            fields("txt_colorStroke_") = PRVColor
                        case PGM
                            fields("txt_colorStroke_") = PGMColor
                        case else
                            fields("txt_colorStroke_") = FREEColor
                    end select
                    ' check direct
                ' ============================================            
                ' ============= Status (playing/paused) =====
                    if player_InpuntNode isnot Nothing then
                        dim state as string = player_InpuntNode.Attributes.GetNamedItem("state").Value
                        select state
                            case "Paused"
                                fields("txt_status_") = paused
                            case "Running"
                                fields("txt_status_") = playing
                            case "Completed"
                                fields("txt_status_") = completed
                        end select

                    end if
                ' ==========================================
                ' ============= FileName AND LIST===================
                    if player_InpuntNode isnot Nothing then
                        player_Type =  player_InpuntNode.Attributes.GetNamedItem("type").Value
                        select player_Type
                            case listType
                                dim nowPlaying as string = ""
                                dim playList as XMLNodeList = player_InpuntNode.SelectSingleNode("list").SelectNodes("item")
                                dim itemIndex as integer = 1
                                dim resultFontSize as integer = defaultListFontSize
                                
                                if playList.count > maxDefaultListItems then
                                    resultFontSize = Math.Floor(defaultListFontSize * maxDefaultListItems /playList.count)
                                    ' console.wl(playList.count & "= >" & resultFontSize.toString())
                                end if
                                for each item as XMLNode in playList                                   
                                    dim fileName as string = Path.GetFileName(item.InnerText)
                                    dim itemIndexString as string = itemIndex.tostring()
                                    dim newLine as string = Environment.NewLine
                                    if item.Attributes.GetNamedItem("selected") isnot Nothing then                                         
                                        dim isSelected as string = item.Attributes.GetNamedItem("selected").Value
                                        if isSelected = "true" then
                                            nowPlaying = fileName
                                            itemIndexString = selectedClipSymbol & itemIndexString
                                        end if 
                                    end if
                                    if itemIndex = playList.count then 
                                        newLine = ""
                                    end if 
                                    fields("txt_listNumbers_") += itemIndexString & "." & newLine
                                    fields("txt_listNames_") += fileName & newLine
                                    itemIndex += 1
                                next
                                fields("txt_playerFileName_") = nowPlaying
                                fields("txt_listFontSize_") = resultFontSize
                            case else 
                                fields("txt_playerFileName_") = "Hidden"
                        end select
                    end if
                ' ==========================================
                ' ========== DURATION/POSITION/IN/OUT============
                    if player_InpuntNode isnot Nothing then
                        ' ================== CHEKNING VARs ================================
                            dim duration as integer = -1 ' -1 as "not yet set"
                            dim position as integer = -1
                            dim markIn as integer = -1
                            dim markOut as integer = -1
                            
                            dim isRemain as boolean = false
                            Boolean.TryParse(playersGUItitle_Input.Text("txt_isRemain_" & index.tostring()), isRemain)
                            
                            
                            if player_InpuntNode.Attributes.GetNamedItem("duration") isnot Nothing then
                                dim durationString as string = player_InpuntNode.Attributes.GetNamedItem("duration").Value
                                duration = Convert.toInt32(durationString)
                            end if
                            if player_InpuntNode.Attributes.GetNamedItem("position") isnot Nothing then
                                dim positionString as string = player_InpuntNode.Attributes.GetNamedItem("position").Value
                                position = Convert.toInt32(positionString)
                            end if
                            if player_InpuntNode.Attributes.GetNamedItem("markIn") isnot Nothing then
                                dim markInString as string = player_InpuntNode.Attributes.GetNamedItem("markIn").Value
                                markIn = Convert.toInt32(markInString)
                            end if
                            if player_InpuntNode.Attributes.GetNamedItem("markOut") isnot Nothing then
                                dim markOutString as string = player_InpuntNode.Attributes.GetNamedItem("markOut").Value
                                markOut = Convert.toInt32(markOutString)
                            end if
                        ' ==================================================================
                        ' ================= DURATION/POSITION ==============================
                            if duration > 0 then
                                dim durTime as timeSpan = TimeSpan.FromMilliseconds(duration)   
                                dim hoursDur as string = ""
                                if durTime.hours <> "00" then 
                                    hoursDur = durTime.hours & ":"
                                end if
                                fields("txt_duration_") = hoursDur & string.Format("{0:00}:{1:00}", durTime.Minutes, durTime.Seconds)
                                
                                if position >= 0 then
                                    dim hoursPos as string = ""
                                    
                                    dim positionForTime as integer = position ' var for Text Conversions

                                    ' ============ DURATION/REMAIN TOGGLE ======================
                                        if isRemain Then
                                            hoursPos = "-"
                                            if markOut > 0 then
                                                positionForTime = markOut - position
                                            else
                                                positionForTime = duration - position
                                            end if 
                                        end if
                                    ' ==========================================================

                                    dim posTime as timeSpan = TimeSpan.FromMilliseconds(positionForTime)   
                                    
                                    if posTime.hours <> "00" then 
                                        hoursPos = hoursPos & posTime.hours & ":" 
                                    end if

                                    fields("txt_curr_") = hoursPos & string.Format("{0:00}:{1:00}", posTime.Minutes, posTime.Seconds)
                                    fields("txt_progress_") = math.Round((position/duration)*progressBarWidth)
                                    ' ============= coloring Remaining time ==================================
                                    dim alarmcolor as string = noAlarmColor
                                        if positionForTime < redAlarmTime then
                                            alarmcolor = redAlarmColor
                                        else
                                            if positionForTime < yellowAlarmTime 
                                                alarmcolor = yellowAlarmcolor
                                            end if
                                        end if
                                        API.Function("SetTextColour", Input:= playersGUItitle_InputName, Value:=alarmcolor, SelectedName:= "txt_curr_" & index.tostring())
                                    '
                                end if  
                            end if  
                        ' ================= ================= ==============================
                        ' ================== IN/OUT ========================================
                            if duration > 0 then

                                dim marks as integer() = { markIn , markOut}

                                dim marksTimeString as string() = { "" , "" }
                                
                                dim marksString as string() = { fields("txt_in_") , fields("txt_out_")}

                                    for n as integer = 0 to marks.length  - 1 
                                        if marks(n) >= 0 then
                                            dim milliseconds as integer = marks(n)
                                            dim markTime as timeSpan = TimeSpan.FromMilliseconds(milliseconds)
                                            dim hoursString as string = ""
                                            if markTime.hours <> "00" then 
                                                hoursString = markTime.hours & ":"
                                            end if
                                            marksTimeString(n) = hoursString & string.Format("{0:00}:{1:00}", markTime.Minutes, markTime.Seconds)
                                            marksString(n) = math.Round((marks(n)/duration)*progressBarWidth)
                                        end if
                                    next 
                               
                                fields("txt_in_") = marksString(0)
                                fields("txt_out_") = marksString(1)
                                
                                if marksTimeString(0) <>"" then
                                    fields("txt_inTime_") = "IN: " & marksTimeString(0) '& "↑"
                                end if
                                if marksTimeString(1) <>"" then
                                    fields("txt_outTime_") = "OUT: " &  marksTimeString(1) '& "↑"
                                end if
                                
                                
                            end if
                        ' ================== ====== ========================================            
                    end if
                ' ==================================================
                ' ==================== AUDIO METERS ================
                    if player_InpuntNode isnot Nothing then


                        dim isHidden as boolean = false 
                        if playersGUItitle_Input.Text("txt_metersVisibility_" & index.tostring()) = "Hidden" then
                            isHidden = false
                        end if

                        dim meters as double() = { -1, -1 }
                        if not isHidden then
                            for n as integer = 0 to meters.length - 1
                                dim attributeName as string = "meterF" & (n+1).toString()
                                
                                if player_InpuntNode.Attributes.GetNamedItem(attributeName) isnot Nothing then

                                    dim metersString as string = player_InpuntNode.Attributes.GetNamedItem(attributeName).Value
                                    
                                    meters(n) = Convert.toDouble(metersString, System.Globalization.CultureInfo.InvariantCulture)
                                    
                                    fields("txt_" & attributeName & "_") = math.Round((meters(n))*heigthOfMeters)
                                else 
                                    isHidden = true
                                end if
                            next
                        end if


                    end if 
                ' ==================================================
                ' ================= API stufff =====================================================================================

                    ' ============== Layers Stuff ====================
                        if not playerInputError then
                            if not previewInputError then

                                    dim playersPreview_InputLayers as XMLNodeList = playersPreview_InpuntNode.SelectNodes("overlay")

                                    if playersPreview_InputLayers.count > 0 then

                                        dim isLayerSet as boolean = false
                                        ' ===================== CHANGING LAYERS ================
                                        ' for each layer as XMLNode in  playersPreview_InputLayers
                                        '     if players(i) <> "" then
                                        '         if layer.Attributes.GetNamedItem("key").Value = player_GUID Then
                                        '             dim oldLayer as integer = convert.toint16(layer.Attributes.GetNamedItem("index").Value + 1 )
                                                    
                                        '             ' console.wl("Debug (line " & me.CurrentLine.tostring() & "): " & players(i) & " is on layer " & oldLayer.tostring())

                                        '             if playerInputLayer <> oldLayer then
                                        '                 Input.Find(playersPreview_InputName).Function("MoveMultiViewOverlay",  oldLayer.tostring() & "," & playerInputLayer.tostring())                                   
                                        '             else 
                                        '                 ' console.wl("Debug (line " & me.CurrentLine.tostring() & "): " & "Target Layer for " & players(i) & " is the same")
                                        '             end if
                                        '             isLayerSet = true
                                        '         end if
                                        '     end if
                                        ' next
                                        ' =============================================================
                                        if not isLayerSet then
                                            Input.Find(playersPreview_InputName).Function("SetMultiViewOverlay", playerInputLayer.tostring & "," & players(i)) 
                                        end if
                                    else
                                        Input.Find(playersPreview_InputName).Function("SetMultiViewOverlay", playerInputLayer.tostring & "," & players(i)) 
                                    end if

                            end if
                        end if 
                    ' ==============================================

                    ' ========== INFO Stuff =============================
                        for each key as string in keyList
                            if  fields(key) <> playersGUItitle_Input.Text( key & index.tostring()) then
                            ' console.wl(key & index.tostring())
                                if  key.toLower().Contains("color") then 
                                    API.Function("SetTextColour", Input:= playersGUItitle_InputName, Value:=fields(key), SelectedName:=key & index.tostring())
                                else      
                                    playersGUItitle_Input.Text( key & index.tostring()) = fields(key)
                                    ' CONSOLE.WL((key & index.tostring()) & " = " & fields(key))
                                end if
                            end if 
                        next

                    ' =================================================
                ' ================= API stufff =====================================================================================
            next
        ' =========================================================

        sleep(interval)
    end while
    end if
' ============================================================
