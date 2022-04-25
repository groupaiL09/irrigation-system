<?php
    include_once("connection.php");
    // check connection to DB
    if (mysqli_connect_errno()){
        echo "1";   //error code #1 = connection failed
        exit();
    }   

    $username = $_POST["username"];
    $dateShow = $_POST["dateShow"];
    
    //check if username exists
    $namecheckquery = "SELECT username, salt, hash  FROM users WHERE username = '" . $username . "';";

    $namecheck = mysqli_query($con, $namecheckquery) or die("Name check query failed"); // error code: name check query failed

    $user_idQuery = "SELECT user_id FROM users WHERE username = '" . $username . "';";
    // $user_idQuery = "SELECT user_id, email FROM users;";

    $user_idCheck = mysqli_query($con, $user_idQuery) or die("User id check query failed"); // error code: name check query failed

    while ($obj = mysqli_fetch_object($user_idCheck)){
        $user_id = $obj->user_id;
    }

    $tempQuery = "SELECT TIME(date) as time, content FROM notifications WHERE user_id = '" . $user_id . "' AND date(date) = '" . $dateShow . "';";

    $tempCheck = mysqli_query($con, $tempQuery) or die("");

    $result = [];
    while ($obj = mysqli_fetch_object($tempCheck)) {
        array_push($result, array("time"=>$obj->time, "content"=>$obj->content)); 
        // array_push($result, array("content"=>$obj->content)); 
    }
    echo json_encode($result);

    //
        // echo mysqli_num_rows($tempCheck);

        // echo $user_idCheck;
        // echo mysqli_num_rows($user_idCheck);

        // if (mysqli_num_rows($namecheck) != 1){
        //     //at least 1 username matches the username being requested
        //     echo "No matching Username"; //error code: No matching Username
        //     exit();
        // }
        // else{
        //     //get login info from query
        //     $existinginfo = mysqli_fetch_assoc($namecheck);
        //     $salt = $existinginfo["salt"];
        //     $hash = $existinginfo["hash"];
            
        //     $loginhash = crypt($password, $salt);
        //     if ($hash != $loginhash){
        //         echo "Incorrect password"; //error code: password does not hash to match table
        //         exit();
        //     }
        // }

        // echo("0");

?>