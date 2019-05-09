export class RaceCalculator {

  constructor() { }

  CalculatePredicion(nextDistance: number, oldDistance: number, oldTime: number ): number
  {
    return (this.Riegel(oldDistance, nextDistance ,oldTime) + this.cameron(oldDistance, nextDistance, oldTime)) / 2;
  }


  FormatTime(totalSeconds: number): string
  {
  var hours = Math.floor(totalSeconds / 3600);
  var minutes = Math.floor((totalSeconds - (hours * 3600)) / 60);
  var seconds = totalSeconds - (hours * 3600) - (minutes * 60);

  // round seconds
  seconds = Math.round(seconds * 100) / 100

  var result = "h:" +  (hours < 10 ? "0" + hours : hours);
  result += " m:" + (minutes < 10 ? "0" + minutes : minutes);
  result += " s:" + (seconds < 10 ? "0" + seconds : seconds);
  return result.toString();
}


  private Riegel(old_distance: number, next_distance: number, old_time: number) {
    var con = 1.06;
    //t2 = t1 * (d2 / d1) ^ 1.06
    // d3 = (d2 / d1)
    var d3 = next_distance / old_distance;
    // w = d3^ 1.06
    var w = Math.pow(d3, con);
    // t2 = t1 * w
    var new_time = old_time * w;
    return new_time;
  }

  private cameron(old_dist: number,next_distance: number, old_time: number) {
    //a = 13.49681 - (0.000030363 * old_dist) + (835.7114 / (old_dist ^ 0.7905))
    //b = 13.49681 - (0.000030363 * new_dist) + (835.7114 / (new_dist ^ 0.7905))
    //new_time = (old_time / old_dist) * (a / b) * new_dist

    // x = (0.000030363 * old_dist)
    var x = (0.000030363 * old_dist);
    // y = (old_dist ^ 0.7905)
    var y = Math.pow(old_dist, 0.7905);
    // z = (835.7114 / y )
    var z = 835.7114 / y;
    var a = 13.49681 - x + z;

    var w = Math.pow(next_distance, 0.7905);
    var v = 835.7114 / w;
    var t = 0.000030363 * next_distance;


    var b = 13.49681 - t + v;

    var new_time = (old_time / old_dist) * (a / b) * next_distance;
    return new_time;

  }

}


//private static double CalculatePredicion(double distances, double oldDistance, int oldtime)
//{
//  return (RaceCalc.calcPredictedTime(oldDistance, Convert.ToDouble(distances), oldtime) + RaceCalc.cameron(oldDistance, Convert.ToDouble(distances), oldtime)) / 2;
//}
